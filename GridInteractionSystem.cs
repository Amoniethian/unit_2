using System;
using System.Collections.Generic;

/// <summary>
/// 格子交互判定系统 - 保护与抢劫逻辑
/// </summary>
namespace GridInteractionSystem
{
    // ==================== 枚举定义 ====================

    /// <summary>
    /// 角色的行为状态
    /// </summary>
    public enum ActionState
    {
        None,       // 无状态
        Protect,    // 保护状态
        Rob         // 抢劫状态
    }

    /// <summary>
    /// 判定结果输出
    /// </summary>
    public enum ActionResult
    {
        None,               // 无输出
        ExecuteProtect,     // 执行保护
        ExecuteRob,         // 执行抢劫
        Intercepted         // 被拦截（抢劫遇到保护）
    }

    /// <summary>
    /// 方向枚举
    /// </summary>
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    // ==================== 格子类 ====================

    /// <summary>
    /// 单个格子
    /// </summary>
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ActionState State { get; set; }
        public string CharacterName { get; set; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            State = ActionState.None;
            CharacterName = null;
        }

        public bool HasCharacter => !string.IsNullOrEmpty(CharacterName);
    }

    // ==================== 网格系统 ====================

    /// <summary>
    /// 网格管理器
    /// </summary>
    public class Grid
    {
        private Cell[,] cells;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            cells = new Cell[width, height];

            // 初始化所有格子
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y] = new Cell(x, y);
                }
            }
        }

        /// <summary>
        /// 获取指定位置的格子
        /// </summary>
        public Cell GetCell(int x, int y)
        {
            if (IsValidPosition(x, y))
                return cells[x, y];
            return null;
        }

        /// <summary>
        /// 检查位置是否有效
        /// </summary>
        public bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        /// <summary>
        /// 获取相邻的四个格子（上下左右）
        /// </summary>
        public List<Cell> GetAdjacentCells(int x, int y)
        {
            var adjacent = new List<Cell>();

            // 上
            if (IsValidPosition(x, y - 1))
                adjacent.Add(cells[x, y - 1]);
            // 下
            if (IsValidPosition(x, y + 1))
                adjacent.Add(cells[x, y + 1]);
            // 左
            if (IsValidPosition(x - 1, y))
                adjacent.Add(cells[x - 1, y]);
            // 右
            if (IsValidPosition(x + 1, y))
                adjacent.Add(cells[x + 1, y]);

            return adjacent;
        }

        /// <summary>
        /// 获取指定方向的格子
        /// </summary>
        public Cell GetCellInDirection(int x, int y, Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return GetCell(x, y - 1);
                case Direction.Down:
                    return GetCell(x, y + 1);
                case Direction.Left:
                    return GetCell(x - 1, y);
                case Direction.Right:
                    return GetCell(x + 1, y);
                default:
                    return null;
            }
        }
    }

    // ==================== 核心判定逻辑 ====================

    /// <summary>
    /// 交互判定器
    /// </summary>
    public class InteractionResolver
    {
        private Grid grid;

        public InteractionResolver(Grid grid)
        {
            this.grid = grid;
        }

        /// <summary>
        /// 判定单个格子的行为结果
        /// </summary>
        /// <param name="x">格子X坐标</param>
        /// <param name="y">格子Y坐标</param>
        /// <returns>判定结果</returns>
        public ActionResult ResolveAction(int x, int y)
        {
            Cell currentCell = grid.GetCell(x, y);
            if (currentCell == null || !currentCell.HasCharacter)
                return ActionResult.None;

            List<Cell> adjacentCells = grid.GetAdjacentCells(x, y);

            switch (currentCell.State)
            {
                case ActionState.Protect:
                    return ResolveProtect(currentCell, adjacentCells);

                case ActionState.Rob:
                    return ResolveRob(currentCell, adjacentCells);

                default:
                    return ActionResult.None;
            }
        }

        /// <summary>
        /// 处理保护状态的判定
        /// 规则：
        /// - 周围有抢劫 → 执行保护
        /// - 周围无抢劫 → 无输出
        /// </summary>
        private ActionResult ResolveProtect(Cell protector, List<Cell> adjacentCells)
        {
            // 检查相邻格子是否有抢劫状态
            bool hasAdjacentRob = false;

            foreach (var cell in adjacentCells)
            {
                if (cell.State == ActionState.Rob)
                {
                    hasAdjacentRob = true;
                    break;
                }
            }

            if (hasAdjacentRob)
            {
                // 周围有抢劫，执行保护
                return ActionResult.ExecuteProtect;
            }
            else
            {
                // 周围无抢劫，无输出
                return ActionResult.None;
            }
        }

        /// <summary>
        /// 处理抢劫状态的判定
        /// 规则：
        /// - 周围无保护 → 执行抢劫
        /// - 周围有保护 → 被拦截
        /// </summary>
        private ActionResult ResolveRob(Cell robber, List<Cell> adjacentCells)
        {
            // 检查相邻格子是否有保护状态
            bool hasAdjacentProtect = false;

            foreach (var cell in adjacentCells)
            {
                if (cell.State == ActionState.Protect)
                {
                    hasAdjacentProtect = true;
                    break;
                }
            }

            if (hasAdjacentProtect)
            {
                // 周围有保护，被拦截
                return ActionResult.Intercepted;
            }
            else
            {
                // 周围无保护，执行抢劫
                return ActionResult.ExecuteRob;
            }
        }

        /// <summary>
        /// 判定指定方向的交互（更精确的版本）
        /// </summary>
        /// <param name="x">当前格子X</param>
        /// <param name="y">当前格子Y</param>
        /// <param name="direction">检测方向</param>
        /// <returns>判定结果</returns>
        public ActionResult ResolveActionInDirection(int x, int y, Direction direction)
        {
            Cell currentCell = grid.GetCell(x, y);
            Cell targetCell = grid.GetCellInDirection(x, y, direction);

            if (currentCell == null || targetCell == null)
                return ActionResult.None;

            // 当前格子是保护，目标方向是抢劫 → 执行保护
            if (currentCell.State == ActionState.Protect &&
                targetCell.State == ActionState.Rob)
            {
                return ActionResult.ExecuteProtect;
            }

            // 当前格子是抢劫，目标方向是保护 → 被拦截
            if (currentCell.State == ActionState.Rob &&
                targetCell.State == ActionState.Protect)
            {
                return ActionResult.Intercepted;
            }

            return ActionResult.None;
        }

        /// <summary>
        /// 解析整个网格的所有交互
        /// </summary>
        public Dictionary<Cell, ActionResult> ResolveAllActions()
        {
            var results = new Dictionary<Cell, ActionResult>();

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    Cell cell = grid.GetCell(x, y);
                    if (cell.HasCharacter && cell.State != ActionState.None)
                    {
                        ActionResult result = ResolveAction(x, y);
                        results[cell] = result;
                    }
                }
            }

            return results;
        }
    }

    // ==================== 使用示例 ====================

    public class Example
    {
        public static void Main()
        {
            // 创建 5x5 网格
            Grid grid = new Grid(5, 5);

            // 设置角色和状态
            // 假设 (2,2) 位置有一个保护者
            var protector = grid.GetCell(2, 2);
            protector.CharacterName = "守卫A";
            protector.State = ActionState.Protect;

            // (2,1) 位置（保护者上方）有一个抢劫者
            var robber = grid.GetCell(2, 1);
            robber.CharacterName = "盗贼B";
            robber.State = ActionState.Rob;

            // 创建判定器
            InteractionResolver resolver = new InteractionResolver(grid);

            // === 场景1: 保护者周围有抢劫 ===
            ActionResult protectorResult = resolver.ResolveAction(2, 2);
            Console.WriteLine($"守卫A的判定结果: {protectorResult}");
            // 输出: ExecuteProtect (因为上方有抢劫者)

            // === 场景2: 抢劫者周围有保护 ===
            ActionResult robberResult = resolver.ResolveAction(2, 1);
            Console.WriteLine($"盗贼B的判定结果: {robberResult}");
            // 输出: Intercepted (因为下方有保护者)

            // === 场景3: 孤立的抢劫者 ===
            var loneRobber = grid.GetCell(4, 4);
            loneRobber.CharacterName = "盗贼C";
            loneRobber.State = ActionState.Rob;

            ActionResult loneRobberResult = resolver.ResolveAction(4, 4);
            Console.WriteLine($"盗贼C的判定结果: {loneRobberResult}");
            // 输出: ExecuteRob (周围没有保护者)

            // === 场景4: 孤立的保护者 ===
            var loneProtector = grid.GetCell(0, 0);
            loneProtector.CharacterName = "守卫D";
            loneProtector.State = ActionState.Protect;

            ActionResult loneProtectorResult = resolver.ResolveAction(0, 0);
            Console.WriteLine($"守卫D的判定结果: {loneProtectorResult}");
            // 输出: None (周围没有抢劫者，无输出)

            Console.WriteLine("\n=== 所有交互判定 ===");
            var allResults = resolver.ResolveAllActions();
            foreach (var kvp in allResults)
            {
                Console.WriteLine($"{kvp.Key.CharacterName} @ ({kvp.Key.X},{kvp.Key.Y}): {kvp.Value}");
            }
        }
    }
}
