// ==================== 登录验证 ====================

function login() {
  const username = document.getElementById('username').value.trim();
  const password = document.getElementById('password').value;
  const errorEl = document.getElementById('error');

  // 清除之前的错误
  errorEl.textContent = '';

  // 正确的凭证 (可根据需要修改)
  const CORRECT_USERNAME = 'user_4712';
  const CORRECT_PASSWORD = 'butterfly2019';

  if (username === CORRECT_USERNAME && password === CORRECT_PASSWORD) {
    // 登录成功
    sessionStorage.setItem('mail-auth', 'true');
    sessionStorage.setItem('mail-user', username);

    // 闪烁效果后跳转
    document.body.style.opacity = '0';
    setTimeout(() => {
      window.location.href = 'inbox.html';
    }, 300);
  } else {
    // 登录失败
    errorEl.textContent = '[ ACCESS DENIED - 访问被拒绝 ]';
    document.querySelector('.login-box').classList.add('shake');

    // 移除抖动效果
    setTimeout(() => {
      document.querySelector('.login-box').classList.remove('shake');
    }, 500);
  }
}

// 回车键提交
document.addEventListener('keypress', function(e) {
  if (e.key === 'Enter') {
    const loginBtn = document.querySelector('button');
    if (loginBtn) {
      login();
    }
  }
});

// ==================== 页面保护 ====================

function checkAuth() {
  if (sessionStorage.getItem('mail-auth') !== 'true') {
    window.location.href = 'index.html';
    return false;
  }
  return true;
}

// ==================== 收件箱功能 ====================

function switchFolder(folder) {
  // 更新标签状态
  document.querySelectorAll('.folder-tab').forEach(tab => {
    tab.classList.remove('active');
  });
  event.target.classList.add('active');

  // 显示/隐藏邮件
  document.querySelectorAll('.mail-item').forEach(item => {
    const itemFolder = item.dataset.folder;
    if (folder === 'all' || itemFolder === folder) {
      item.style.display = 'block';
    } else {
      item.style.display = 'none';
    }
  });
}

// ==================== 登出 ====================

function logout() {
  sessionStorage.removeItem('mail-auth');
  sessionStorage.removeItem('mail-user');
  window.location.href = 'index.html';
}
