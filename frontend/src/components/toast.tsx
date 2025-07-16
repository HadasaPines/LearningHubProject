import React from "react";
import { FaCheckCircle, FaTimesCircle } from "react-icons/fa";
import styles from './Toast.module.scss';

interface ToastProps {
  type: "success" | "error";
  message: string;
}

const Toast: React.FC<ToastProps> = ({ type, message }) => {
  return (
    <div className={`${styles.toast} ${styles[type]}`}>
      {type === "success" ? <FaCheckCircle /> : <FaTimesCircle />}
      <span>{message}</span>
    </div>
  );
};

export default Toast;
