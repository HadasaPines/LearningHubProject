import React from "react";
import { FaCheckCircle, FaTimesCircle } from "react-icons/fa";
import styles from './toast.module.scss';

interface ToastProps {
  type: "success" | "error";
  message: string;
  children?: React.ReactNode;
}

const Toast: React.FC<ToastProps> = ({ type, message, children }) => {



  return (
    <div className={`${styles.toast} ${styles[type]}`}>
      {type === "success" ? <FaCheckCircle /> : <FaTimesCircle />}
      <span>{message}</span>

      {children && (
        <div >
          {React.Children.map(children, (child, index) => (
            <div key={index} className="child-wrapper">
              {child}
            </div>
          ))}
        </div>
      )}

    </div>
  );
};

export default Toast;
