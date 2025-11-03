import React, { useEffect, useState } from "react";
import type { Subscription } from "../../models/subscriptionModel";
import type { StudentSubscription } from "../../models/studentSubscriptionModel";
import type { User } from "../../models/userModel";
import styles from './buyStudentSubscriptions.module.scss';
import { FaHeart, FaFileAlt, FaGlobe } from 'react-icons/fa';

import {
  getAllSubscriptions,
  addStudentSubscription,
  checkAddStudentSubscription,
} from "../../services/api";
import PaymentOverlay from "../paymentOverlay";
import { parseApiError } from "../../utils/apiErrorParser";
import Toast from "../../components/toast";
import StudentHeader from "../../components/student/studentHeader"

const BuyStudentSubscriptions: React.FC = () => {
  const user = localStorage.getItem("user");
  if (!user) return <div>User not logged in</div>;
  const parsedUser: User = JSON.parse(user);

  const [availableSubscriptions, setAvailableSubscriptions] = useState<Subscription[]>([]);
  const [selectedSubscription, setSelectedSubscription] = useState<Subscription | null>(null);
  const [paymentOpen, setPaymentOpen] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);
  const [newStudentSub, setNewStudentSub] = useState<Omit<StudentSubscription, "studentSubscriptionId">>({
    studentId: parsedUser.userId,
    lessonsUsed: 0,
    isActive: true,
    subscriptionId: 0,
  });

  useEffect(() => {
    loadSubscriptions();
  }, []);

  useEffect(() => {
    if (error || success) {
      const timeout = setTimeout(() => {
        setError(null);
        setSuccess(null);
      }, 4000);
      return () => clearTimeout(timeout);
    }
  }, [error, success]);

  const loadSubscriptions = async () => {
    try {
      const [subs] = await Promise.all([getAllSubscriptions()]);
      setAvailableSubscriptions(subs.data);
    } catch (err) {
      setError("Failed to load subscriptions: " + parseApiError(err));
    }
  };

  const handleBuy = async (subscription: Subscription) => {
    if (!subscription.subscriptionId) {
      setError("Invalid subscription selected");
      return;
    }

    const tempSub: Omit<StudentSubscription, "studentSubscriptionId"> = {
      studentId: parsedUser.userId,
      lessonsUsed: 0,
      isActive: true,
      subscriptionId: subscription.subscriptionId,
    };

    try {
      const res = await checkAddStudentSubscription(tempSub);
      if (res.data === true) {
        setNewStudentSub(tempSub);
        setSelectedSubscription(subscription);
        setPaymentOpen(true);
        setError(null);
      } else {
        setError("You already have an active subscription or the selected one is invalid.");
      }
    } catch (err) {
      setError("Error checking subscription: " + parseApiError(err));
    }
  };

  const handlePaymentSuccess = async () => {
    setPaymentOpen(false);
    if (!selectedSubscription) return;

    try {
      await addStudentSubscription(newStudentSub);
      setSelectedSubscription(null);
      loadSubscriptions();
      setSuccess("Subscription purchased successfully!");
    } catch (err) {
      setError("Error purchasing subscription: " + parseApiError(err));
    }
  };

  return (
    <>
      <StudentHeader /> 
      {error && <Toast type="error" message={error} />}
      {success && <Toast type="success" message={success} />}

      <div className={styles.container} style={{ marginTop: "5rem" }}>
        <div className={styles.circle2}></div>
        <div className={styles.circle3}></div>

        <h3 className={styles.title}>Available Subscriptions</h3>
        <div className={styles.subscriptionsList}>
          {availableSubscriptions.map((sub) => (
            <div key={sub.subscriptionId} className={styles.card}>
              {sub.name.toLowerCase().includes("premium") && <div className={styles.icon}><FaGlobe /></div>}
              {sub.name.toLowerCase().includes("basic") && <div className={styles.icon}><FaFileAlt /></div>}
              {sub.name.toLowerCase().includes("advance") && <div className={styles.icon}><FaHeart /></div>}

              <h4>{sub.name}</h4>
              <div className={styles.price}>₪ {sub.price}</div>
              <div className={styles.description}>{sub.description}</div>
              <div className={styles.features}>
                {sub.lessonCount && <p><strong>Total Lessons:</strong> {sub.lessonCount}</p>}
              </div>

              <button className={styles.buyBtn} onClick={() => handleBuy(sub)}>Select Plan</button>
            </div>
          ))}
        </div>

        <PaymentOverlay
          userId={parsedUser.userId}
          open={paymentOpen}
          onClose={() => setPaymentOpen(false)}
          amount={selectedSubscription?.price || 0}
          onPaymentSuccess={handlePaymentSuccess}
        />
      </div>
    </>
  );
};

export default BuyStudentSubscriptions;
