// ==================== 搜索功能 ====================

function search() {
  const query = document.getElementById('search-input').value.trim().toLowerCase();

  if (query.includes('monarch') || query.includes('事故')) {
    // 已经在正确的页面
    alert('已显示搜索结果: Project Monarch 事故报告');
  } else {
    alert('未找到相关文章。\n\n提示: 尝试搜索 "Project Monarch"');
  }
}

// ==================== 查看编辑历史 ====================

function viewHistory() {
  const code = prompt('请输入编辑者权限码以查看历史版本:');

  if (code === null) {
    // 用户点击取消
    return;
  }

  // 支持多种格式
  const validCodes = ['monarch_v3', 'monarch_V3', 'MONARCH_V3', 'v3', 'V3'];

  if (validCodes.includes(code) || code.toLowerCase() === 'monarch_v3') {
    window.location.href = 'history.html';
  } else {
    alert('⚠ 权限验证失败\n\n错误代码: AUTH_DENIED\n\n提示: 权限码格式为 [项目名]_v[版本号]');
  }
}

// ==================== 查看讨论页 ====================

function viewDiscussion() {
  const id = prompt('请输入员工编号以访问讨论页:');

  if (id === null) {
    // 用户点击取消
    return;
  }

  if (id === '4712') {
    window.location.href = 'discussion.html';
  } else {
    alert('⚠ 员工编号不存在或无权限\n\n错误代码: USER_NOT_FOUND');
  }
}

// ==================== 回车键搜索 ====================

document.addEventListener('DOMContentLoaded', function() {
  const searchInput = document.getElementById('search-input');
  if (searchInput) {
    searchInput.addEventListener('keypress', function(e) {
      if (e.key === 'Enter') {
        search();
      }
    });
  }
});
