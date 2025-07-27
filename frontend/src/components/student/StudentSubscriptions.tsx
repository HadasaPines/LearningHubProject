
import React, { useEffect, useState } from "react";
import type { StudentSubscription } from "../../models/studentSubscriptionModel";
import { getStudentSubscriptionById, getSubscriptionById } from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";
import type { User } from "../../models/userModel";
import styles from "./StudentSubscriptions.module.scss";
import type { Subscription } from "../../models/subscriptionModel";
import Toast from "../toast"; 

const StudentSubscriptions: React.FC = () => {
  const user = localStorage.getItem("user");
  if (!user) return <div>User not logged in</div>;
  const parsedUser: User = JSON.parse(user);

  const [studentSubscriptions, setStudentSubscriptions] = useState<StudentSubscription[]>([]);
  const [subscriptions, setSubscriptions] = useState<Subscription[]>([]);
  const [error, setError] = useState<string | null>(null);

  const loadSubscriptions = async () => {
    try {
      const studentSubs = await getStudentSubscriptionById(parsedUser.userId);
      setStudentSubscriptions(studentSubs.data);
    } catch (err) {
      setError("Failed to load subscriptions: " + parseApiError(err));
    }
  };

  const getSubscription = async (id: number) => {
    try {
      const sub = await getSubscriptionById(id);
      setSubscriptions(prevSubscriptions => [...prevSubscriptions, sub.data]);
    } catch (err) {
      setError("Failed to load subscription: " + parseApiError(err));
    }
  };

  useEffect(() => {
    loadSubscriptions();
  }, []);

  useEffect(() => {
    studentSubscriptions.forEach(sub => {
      getSubscription(sub.subscriptionId);
    });
  }, [studentSubscriptions]);

  const activeSubscription = studentSubscriptions.find(sub => sub.isActive);

  return (
    <div>
      {error && <Toast type="error" message={error} />}
      
    <div className={styles.container}>


    

      {activeSubscription ? (
        <div className={styles.activeSubscription}>
          <h3>Active Subscription</h3>
          <div className={styles.subscriptionCard}>
            <p><strong>Subscription Name:</strong> {subscriptions.find(sub => sub.subscriptionId === activeSubscription.subscriptionId)?.name}</p>
            <p><strong>Total Lessons:</strong> {subscriptions.find(sub => sub.subscriptionId === activeSubscription.subscriptionId)?.lessonCount}</p>
            <p><strong>Lessons Used:</strong> {activeSubscription.lessonsUsed}</p>
          </div>
        </div>
      ) : (
        <div className={styles.noActiveSubscription}>
          <p>No active subscription found.</p>
          <button onClick={() => window.location.href = '/student/Buysubscriptions'}>Go to Buy Subscriptions</button>
        </div>
      )}

      <div className={styles.inactiveSubscriptions}>
        <div className={styles.subscriptionGrid}>
          {studentSubscriptions.filter(sub => !sub.isActive).map((sub) => {
            const subDetails = subscriptions.find((subItem) => subItem.subscriptionId === sub.subscriptionId);
            return (
              <div key={sub.studentSubscriptionId} className={styles.inactiveCard}>
                {subDetails ? (
                  <>
                    <p><strong>Subscription Name:</strong> {subDetails.name}</p>
                    <p><strong>Total Lessons:</strong> {subDetails.lessonCount}</p>
                    <p><strong>Lessons Used:</strong> {sub.lessonsUsed}</p>
                    <div className={styles.watermark}>
                      <div className={styles.stamp}>Inactive</div>
                    </div>
                  </>
                ) : (
                  <p>Loading subscription details...</p>
                )}
              </div>
            );
          })}
        </div>
      </div>
    </div>
    </div>
  );
};

export default StudentSubscriptions;
