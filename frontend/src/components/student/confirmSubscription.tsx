import React from "react";
import styles from "./SubscriptionError.module.scss";

interface Props {
  onConfirm: () => void;
  onCancel: () => void;
}

const ConfirmSubscriptionModal: React.FC<Props> = ({ onConfirm, onCancel }) => {
  return (
    <div className={styles.overlay}>
      <div className={styles.modal}>
        <button className={styles.closeButton} onClick={onCancel}>
          ×
        </button>

        <h2>Confirm Subscription Usage</h2>
        <p>
          Are you sure you want to use one lesson from your active subscription
          for this registration?
        </p>

        <div className={styles.buttons}>
          <button className={styles.subscribe} onClick={onConfirm}>
            Yes, Use Subscription
          </button>
          <button className={styles.pay} onClick={onCancel}>
            Cancel
          </button>
        </div>
      </div>
    </div>
  );
};

export default ConfirmSubscriptionModal;
