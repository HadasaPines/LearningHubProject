import { useState } from "react";
import LoginForm from "../components/student/login";
import RegisterForm from "../components/student/register";
import styles from "./authPage.module.scss";
import Toast from "../components/toast";

const AuthPage = () => {
  const [isLogin, setIsLogin] = useState(true);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const toggleForm = () => {
    setIsLogin(!isLogin);
  };

  const handleToast = (type: "error" | "success", message: string) => {
    if (type === "error") {
      setErrorMessage(message);
      setTimeout(() => setErrorMessage(null), 3000);
    } else {
      setSuccessMessage(message);
      setTimeout(() => setSuccessMessage(null), 3000);
    }
  };

  return (
    <div className={`${styles["auth-container"]} ${isLogin ? styles["login-active"] : styles["register-active"]}`}>
      {errorMessage && <Toast type="error" message={errorMessage} />}
      {successMessage && <Toast type="success" message={successMessage} />}

      <div className={styles.circle1}></div>
      <div className={styles.circle2}></div>

      <div className={styles["form-wrapper"]}>
        <div className={`${styles["form-panel"]} ${styles["login-panel"]}`}>
          <div className={styles["form-content"]}>
            <div className={styles["form-header"]}>
              <h2>Welcome Back!</h2>
              <p>Don't have an account? <button onClick={toggleForm}>Sign Up</button></p>
            </div>
            <LoginForm onToast={handleToast} />
          </div>
        </div>

        <div className={`${styles["form-panel"]} ${styles["register-panel"]}`}>
          <div className={styles["form-content"]}>
            <div className={styles["form-header"]}>
              <h2>New Here?</h2>
              <p>Already have an account? <button onClick={toggleForm}>Sign In</button></p>
            </div>
            <RegisterForm onToast={handleToast} />
          </div>
        </div>

        <div className={styles["cover-panel"]}></div>
        <div className={styles["cover-content"]}></div>
      </div>
    </div>
  );
};

export default AuthPage;
