//DOM
let signupBtn = document.querySelector('.signup-btn')
let loginBtn = document.querySelector('.login-btn')
let form = document.querySelector('.form-section')

window.onload = function () {
    init()
    signupBtn.onclick = function () {
        showSignupForm()
        loginBtn.style.display = ''
        signupBtn.style.display = 'none'
        form.setAttribute("action", "/Member/Signup")
    }
    loginBtn.onclick = function () {
        showLoginForm()
        loginBtn.style.display = 'none'
        signupBtn.style.display = ''
        form.setAttribute("action", "/Member/Login")
    }
}

function init() {
    loginBtn.style.display = 'none'
    showLoginForm()
}

function showSignupForm() {
    form.innerHTML = `
        <div class="input-group mb-4">
            <span class="input-group-text icon_social bg-white"><i class="fas fa-envelope"></i></span>
            <input name="Email" type="email" class="form-control border-start-0 input_noshadow" placeholder="信箱" required>
        </div>

        <div class="input-group mb-4">
            <span class="input-group-text icon_social bg-white"><i class="fas fa-user-circle"></i></span>
            <input name="Realname" type="text" class="form-control border-start-0 input_noshadow" placeholder="真實姓名" required minlength="2" maxlength="64">
        </div>

        <div class="input-group mb-4">
            <span class="input-group-text icon_social bg-white"><i class="fas fa-user-circle"></i></span>
            <input name="Nickname" type="text" class="form-control border-start-0 input_noshadow" placeholder="暱稱" required maxlength="10">
        </div>

        <div class="input-group mb-4">
            <span class="input-group-text icon_social bg-white"><i class="fas fa-phone"></i></span>
            <input name="Phone" type="tel" class="form-control border-start-0 input_noshadow" placeholder="手機" required>
        </div>

        <div class="input-group">
            <span class="input-group-text icon_social bg-white"><i class="fas fa-key"></i></span>
            <input name="Password" type="password" class="form-control border-start-0 border-end-0 input_noshadow" placeholder="密碼" required minlength="6" maxlength="64">
            <button class="input-group-text icon_social bg-white"><i class="fas fa-eye-slash"></i></button>
        </div>
        <span class="text-end fs-12 text-secondary fw-normal">6-64位英數混合，英文需區分大小寫</span>

        <div class="input-group">
            <span class="input-group-text icon_social bg-white"><i class="fas fa-key"></i></span>
            <input name="PasswordCheck" type="password" class="form-control border-start-0 border-end-0 input_noshadow" placeholder="再次輸入密碼" required minlength="6" maxlength="64">
            <button class="input-group-text icon_social bg-white"><i class="fas fa-eye-slash"></i></button>
        </div>
        <span class="text-end fs-12 text-secondary fw-normal">6-64位英數混合，英文需區分大小寫</span>

        <button class="btn bg_pink text-white" type="submit">註冊</button>
        
    `
}

function showLoginForm() {
    form.innerHTML = `
        <div class="input-group mb-4">
            <span class="input-group-text icon_social bg-white"><i class="fas fa-envelope"></i></span>
            <input name="Account" type="email" class="form-control border-start-0 input_noshadow email-input" placeholder="信箱" required>
            
        </div>

        <div class="input-group">
            <span class="input-group-text icon_social bg-white"><i class="fas fa-key"></i></span>
            <input name="Password" type="password" class="form-control border-start-0 border-end-0 input_noshadow password-input" placeholder="密碼" required>
            <button class="input-group-text icon_social bg-white"><i class="fas fa-eye-slash"></i></button>
        </div>
        <span class="text-end fs-12 text-secondary fw-normal mb-2">6-64位英數混合，英文需區分大小寫</span>
        <button class="btn bg_blue text-white submit-btn" type="submit">登入</button>
        
    `
}