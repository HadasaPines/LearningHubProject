import React from "react";
import styles from "./SubscriptionError.module.scss";
import { useNavigate } from "react-router-dom";

interface Props {
  onClose: () => void;
  onSinglePayment: () => void;
}

const SubscriptionErrorModal: React.FC<Props> = ({ onClose, onSinglePayment }) => {
  const navigate = useNavigate();

  return (
    <div className={styles.overlay}>
      <div className={styles.modal}>
       
        <button className={styles.closeButton} onClick={onClose}>
          ×
        </button>

        <h2>No Active Subscription</h2>
        <p>You need an active subscription to register for lessons.</p>
        <div className={styles.buttons}>
          <button
            className={styles.subscribe}
            onClick={() => navigate("/student/buySubscriptions")}
          >
            Buy Subscription
          </button>
          <button className={styles.pay} onClick={onSinglePayment}>
            Pay for Single Lesson
          </button>
        </div>
      </div>
    </div>
  );
};

export default SubscriptionErrorModal;
