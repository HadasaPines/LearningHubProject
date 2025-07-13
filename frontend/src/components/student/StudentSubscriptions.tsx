import React, { useEffect, useState } from "react";
import type { Subscription } from "../../models/subscriptionModel";
import type { StudentSubscription } from "../../models/studentSubscriptionModel";
import type { User } from "../../models/userModel";
import {
  getAllSubscriptions,
  getStudentSubscriptionById,
  addStudentSubscription,
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

  const handleBuy = (subscription: Subscription) => {
    setSelectedSubscription(subscription);
    setNewStudentSub((prev) => ({
      ...prev,
      subscriptionId: subscription.subscriptionId ?? 0,
    }));
  };

  useEffect(() => {
    if (selectedSubscription) {
      setPaymentOpen(true);
    }
  }, [selectedSubscription]);

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
    <div className="p-6">
      <h2 className="text-2xl font-bold mb-4">Your Subscriptions</h2>

      {error && <div className="text-red-600 mb-2">{error}</div>}
      {success && <div className="text-green-600 mb-2">{success}</div>}

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-8">
        {studentSubscriptions.length === 0 ? (
          <p>No subscriptions found.</p>
        ) : (
          studentSubscriptions.map((sub) => (
            <div key={sub.studentSubscriptionId} className="border p-4 rounded shadow">
              <p><strong>Subscription ID:</strong> {sub.subscriptionId}</p>
              <p><strong>Lessons Used:</strong> {sub.lessonsUsed}</p>
              <p><strong>Status:</strong> {sub.isActive ? "Active" : "Inactive"}</p>
            </div>
          ))
        )}
      </div>

      <h3 className="text-xl font-semibold mb-2">Buy New Subscription</h3>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        {availableSubscriptions.map((sub) => (
          <div key={sub.subscriptionId} className="border p-4 rounded shadow flex flex-col justify-between">
            <div>
              <h4 className="font-bold text-lg">{sub.name}</h4>
              <p>{sub.description}</p>
              <p><strong>Price:</strong> {sub.price} ₪</p>
              {sub.lessonCount && <p><strong>Lessons:</strong> {sub.lessonCount}</p>}
            </div>
            <button
              onClick={() => handleBuy(sub)}
              className="mt-4 bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700"
            >
              Buy
            </button>
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
