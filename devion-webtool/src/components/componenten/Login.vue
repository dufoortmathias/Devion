<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <div id="fake-nav" class="c-buttons">
    <!-- <button href="#register" @click="open('register', $event)" class="c-button">Register</button> -->
    <button href="#login" @click="open('login', $event)" class="c-button">Login</button>
  </div>
  <div class="user-modal-container" :class="{ active: active }" id="login-modal" @click="close">
    <div class="user-modal">
      <ul class="form-switcher">
        <li @click="flip('register', $event)"><a href="" id="register-form">Register</a></li>
        <li @click="flip('login', $event)"><a href="" id="login-form">Login</a></li>
      </ul>
      <div class="form-register" :class="{ active: active == 'register' }" id="form-register">
        <div class="error-message" v-text="registerError"></div>
        <input
          type="text"
          name="name"
          placeholder="Name"
          v-model="registerName"
          @keyup.enter="submit('register', $event)"
        />
        <input
          type="email"
          name="email"
          placeholder="Email"
          v-model="registerEmail"
          @keyup.enter="submit('register', $event)"
        />
        <input
          type="password"
          name="password"
          placeholder="Password"
          v-model="registerPassword"
          @keyup.enter="submit('register', $event)"
        />
        <input
          type="submit"
          :class="{ disabled: submitted == 'register' }"
          @click="submit('register', $event)"
          v-model="registerSubmit"
          id="registerSubmit"
        />
        <div class="links">
          <a href="" @click="flip('login', $event)">Already have an account?</a>
        </div>
      </div>
      <div class="form-login" :class="{ active: active == 'login' }" id="form-login">
        <div class="error-message" v-text="loginError"></div>
        <input
          type="email"
          name="email"
          placeholder="Email"
          v-model="loginUser"
          @keyup.enter="submit('login', $event)"
        />
        <input
          type="password"
          name="password"
          placeholder="Password"
          v-model="loginPassword"
          @keyup.enter="submit('login', $event)"
        />
        <input
          type="submit"
          :class="{ disabled: submitted == 'login' }"
          @click="submit('login', $event)"
          v-model="loginSubmit"
          id="loginSubmit"
        />
        <div class="links">
          <a href="" @click="flip('password', $event)">Forgot your password?</a>
        </div>
      </div>
      <div class="form-password" :class="{ active: active == 'password' }" id="form-password">
        <div class="error-message" v-text="passwordError"></div>
        <input
          type="text"
          name="email"
          placeholder="Email"
          v-model="passwordEmail"
          @keyup.enter="submit('password', $event)"
        />
        <input
          type="submit"
          :class="{ disabled: submitted == 'password' }"
          @click="submit('password', $event)"
          v-model="passwordSubmit"
          id="passwordSubmit"
        />
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'loginPopup',
  data() {
    return {
      modal_submit_register: 'Register',
      modal_submit_password: 'Reset Password',
      modal_submit_login: 'Login',

      active: null,
      submitted: null,

      // Submit button text
      registerSubmit: this.modal_submit_register,
      passwordSubmit: this.modal_submit_password,
      loginSubmit: this.modal_submit_login,

      // Modal text fields
      registerName: '',
      registerEmail: '',
      registerPassword: '',
      loginUser: '',
      loginPassword: '',
      passwordEmail: '',

      // Modal error messages
      registerError: '',
      loginError: '',
      passwordError: ''
    }
  },
  methods: {
    open(type, e) {
      e.preventDefault()
      this.active = type
      document.addEventListener('click', this.closeModal)
    },
    close(e) {
      e.preventDefault()
      this.active = null
      document.removeEventListener('click', this.closeModal)
    },
    closeModal(e) {
      let container = document.querySelector('.user-modal-container')
      let target = e.target
      if (!container.contains(target) && !target.classList.contains('user-modal')) {
        this.active = null
        document.removeEventListener('click', this.closeModal)
      }
    },
    flip: function (which, e) {
      e.preventDefault()
      if (which !== this.active) {
        this.active = which
      }
    },
    submit: function (which, e) {
      e.preventDefault()
      this.submitted = which
      var data = {
        form: which
      }

      switch (which) {
        case 'register':
          data.name = this.registerName
          data.email = this.registerEmail
          data.password = this.registerPassword
          this.$set('registerSubmit', 'Registering...')
          break
        case 'login':
          data.user = this.loginUser
          data.password = this.loginPassword
          this.$set('loginSubmit', 'Logging In...')
          break
        case 'password':
          data.email = this.passwordEmail
          this.$set('passwordSubmit', 'Resetting Password...')
          break
      }

      // TODO: submit our `data` variable
    }
  }
}
</script>

<style>
.user-modal-container * {
  box-sizing: border-box;
}

.user-modal-container {
  position: fixed;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  opacity: 0;
  visibility: hidden;
  cursor: pointer;
  overflow-y: auto;
  z-index: 3;
  font-family: 'Lato', 'Helvetica Neue', 'Helvetica', 'Arial', 'sans-serif';
  font-size: 14px;
  background-color: rgba(17, 17, 17, 0.9);
  -webkit-transition: all 0.25s linear;
  -moz-transition: all 0.25s linear;
  -o-transition: all 0.25s linear;
  -ms-transition: all 0.25s linear;
  transition: all 0.25s linear;
}

.user-modal-container.active {
  opacity: 1;
  visibility: visible;
}

.user-modal-container .user-modal {
  position: relative;
  margin: 50px auto;
  width: 90%;
  max-width: 500px;
  background-color: #f6f6f6;
  cursor: initial;
}

.user-modal-container .form-login,
.user-modal-container .form-register,
.user-modal-container .form-password {
  padding: 75px 25px 25px;
  display: none;
}

.user-modal-container .form-login.active,
.user-modal-container .form-register.active,
.user-modal-container .form-password.active {
  display: block;
}

.user-modal-container ul.form-switcher {
  margin: 0;
  padding: 0;
}

.user-modal-container ul.form-switcher li {
  list-style: none;
  display: inline-block;
  width: 50%;
  float: left;
  margin: 0;
}

.user-modal-container ul.form-switcher li a {
  width: 100%;
  display: block;
  height: 50px;
  line-height: 50px;
  color: #666666;
  background-color: #dddddd;
  text-align: center;
}

.user-modal-container ul.form-switcher li a.active {
  color: #000000;
  background-color: #f6f6f6;
}

.user-modal-container input {
  width: 100%;
  padding: 10px;
  margin-bottom: 10px;
  border: 1px solid #eeeeee;
}

.user-modal-container input[type='submit'] {
  color: #f6f6f6;
  border: 0;
  margin-bottom: 0;
  background-color: #3fb67b;
  cursor: pointer;
}

.user-modal-container input[type='submit']:hover {
  background-color: #3aa771;
}

.user-modal-container input[type='submit']:active {
  background-color: #379d6b;
}

.user-modal-container .links {
  text-align: center;
  padding-top: 25px;
}

.user-modal-container .links a {
  color: #3fb67b;
}

.user-modal-container input[type='submit'].disabled {
  background-color: #98d6b7;
}

.c-button {
  font-size: 16px;
  font-weight: 700;
  border-radius: var(--global-borderRadius);
  border: var(--global-borderWidth) solid var(--global-color-alpha-dark);
  background-color: var(--global-color-alpha);
  color: #fff;
  padding: calc(var(--global-baseline) * 1.5 - var(--global-borderWidth))
    var(--global-whitespace-md);
  line-height: calc(var(--global-baseline) * 3);
  width: 40%;
  outline: none;
  transition: all var(--global-transitionTiming-alpha);
  cursor: pointer;
}

.c-button:hover {
  background-color: var(--global-color-alpha-light);
}

.c-button:focus {
  box-shadow: 0 0 0 3px var(--global-color-alpha-x-transparent);
}

.c-button:active {
  background-color: var(--global-color-alpha-dark);
  box-shadow: none;
}

.c-button:disabled {
  background-color: var(--global-color-alpha-x-light);
  border-color: var(--global-color-alpha-x-light);
  color: var(--global-color-neutral-dark);
  cursor: not-allowed;
}

.c-buttons {
    display: flex;
    justify-content: end;
    margin-top: 1rem;
}
</style>