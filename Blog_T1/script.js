// ==================== 邀请码验证 ====================

function enter() {
  const codeInput = document.getElementById('invite-code');
  const errorMsg = document.getElementById('error-msg');
  const code = codeInput.value.trim();

  // 清除之前的错误
  errorMsg.textContent = '';

  if (!code) {
    errorMsg.textContent = '[ ERROR: 请输入邀请码 ]';
    return;
  }

  // 有效的邀请码 (支持多种格式)
  const validCodes = [
    'truth2019',
    'TRUTH2019',
    'Truth2019',
    'truth_2019',
    'TRUTH_2019'
  ];

  if (validCodes.includes(code) || code.toLowerCase() === 'truth2019') {
    // 验证成功
    sessionStorage.setItem('blog-auth', 'true');

    // 成功动画
    document.querySelector('.terminal').style.borderColor = '#00ff00';
    errorMsg.style.color = '#00ff00';
    errorMsg.textContent = '[ ACCESS GRANTED - 验证成功 ]';

    // 跳转
    setTimeout(() => {
      window.location.href = 'posts.html';
    }, 800);
  } else {
    // 验证失败
    errorMsg.textContent = '[ ACCESS DENIED - 邀请码无效 ]';
    document.querySelector('.terminal').classList.add('shake');

    setTimeout(() => {
      document.querySelector('.terminal').classList.remove('shake');
    }, 400);
  }
}

// ==================== 页面保护 ====================

function checkBlogAuth() {
  if (sessionStorage.getItem('blog-auth') !== 'true') {
    window.location.href = 'index.html';
    return false;
  }
  return true;
}

// ==================== 回车键提交 ====================

document.addEventListener('DOMContentLoaded', function() {
  const codeInput = document.getElementById('invite-code');
  if (codeInput) {
    codeInput.addEventListener('keypress', function(e) {
      if (e.key === 'Enter') {
        enter();
      }
    });

    // 自动聚焦
    codeInput.focus();
  }

  // 随机在线人数
  const onlineCount = document.getElementById('online-count');
  if (onlineCount) {
    onlineCount.textContent = Math.floor(Math.random() * 10) + 3;
  }
});

// ==================== 自毁倒计时 (视觉效果) ====================

function startCountdown(elementId, seconds) {
  const el = document.getElementById(elementId);
  if (!el) return;

  let remaining = seconds;

  const timer = setInterval(() => {
    remaining--;
    const mins = Math.floor(remaining / 60);
    const secs = remaining % 60;
    el.textContent = `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;

    if (remaining <= 0) {
      clearInterval(timer);
      el.textContent = '00:00';
      // 只是视觉效果，不会真的删除
    }
  }, 1000);
}

// ==================== 登出 ====================

function blogLogout() {
  sessionStorage.removeItem('blog-auth');
  window.location.href = 'index.html';
}
