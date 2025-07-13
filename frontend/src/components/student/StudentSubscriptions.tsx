import React, { useEffect, useState } from "react";
import type { Subscription } from "../../models/subscriptionModel";
import type { StudentSubscription } from "../../models/studentSubscriptionModel";
import type { User } from "../../models/userModel";
import {
  getAllSubscriptions,
  getStudentSubscriptionById,
  addStudentSubscription,
  checkAddStudentSubscription, 
} from "../../services/api";
import PaymentOverlay from "../paymentOverlay";
import { parseApiError } from "../../utils/apiErrorParser";

const StudentSubscriptions: React.FC = () => {
  const user = localStorage.getItem("user");
  if (!user) return <div>User not logged in</div>;
  const parsedUser: User = JSON.parse(user);

  const [availableSubscriptions, setAvailableSubscriptions] = useState<Subscription[]>([]);
  const [studentSubscriptions, setStudentSubscriptions] = useState<StudentSubscription[]>([]);
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

  const loadSubscriptions = async () => {
    try {
      const [subs, studentSubs] = await Promise.all([
        getAllSubscriptions(),
        getStudentSubscriptionById(parsedUser.userId),
      ]);
      setAvailableSubscriptions(subs.data);
      setStudentSubscriptions(studentSubs.data);
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
      setSuccess("Subscription purchased successfully");
      setSelectedSubscription(null);
      loadSubscriptions();
    } catch (err) {
      setError("Error purchasing subscription: " + parseApiError(err));
    }
  };

  return (
    <div>
      <h2>Your Subscriptions</h2>

      {error && <div style={{ color: "red" }}>{error}</div>}
      {success && <div style={{ color: "green" }}>{success}</div>}

      <div>
        {studentSubscriptions.length === 0 ? (
          <p>No subscriptions found.</p>
        ) : (
          studentSubscriptions.map((sub) => (
            <div key={sub.studentSubscriptionId}>
              <p><strong>Subscription ID:</strong> {sub.subscriptionId}</p>
              <p><strong>Lessons Used:</strong> {sub.lessonsUsed}</p>
              <p><strong>Status:</strong> {sub.isActive ? "Active" : "Inactive"}</p>
            </div>
          ))
        )}
      </div>

      <h3>Buy New Subscription</h3>
      <div>
        {availableSubscriptions.map((sub) => (
          <div key={sub.subscriptionId}>
            <div>
              <h4>{sub.name}</h4>
              <p>{sub.description}</p>
              <p><strong>Price:</strong> {sub.price} ₪</p>
              {sub.lessonCount && <p><strong>Lessons:</strong> {sub.lessonCount}</p>}
            </div>
            <button onClick={() => handleBuy(sub)}>Buy</button>
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
  );
};

export default StudentSubscriptions;
