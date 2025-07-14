import { useEffect, useState } from "react";
import { getPaymentsByUserId } from "../../services/api";
import type { Payment } from "../../models/paymentModel";

const StudentPayments = () => {
  const [payments, setPayments] = useState<Payment[]>([]);
  const [loading, setLoading] = useState(true);
  const user = JSON.parse(localStorage.getItem("user") || "{}");
  const userId = user?.userId;

  useEffect(() => {
    console.log("Fetching payments for userId:", userId);
    setLoading(true);
    if (userId) {
      getPaymentsByUserId(Number(userId))
        .then(setPayments)
        .finally(() => setLoading(false));
    }
  }, [userId]);

  if (loading) return <p>Loading payments...</p>;

  return (
    <div>
      <h2>Payment History</h2>
      <table>
        <thead>
          <tr>
            <th>Amount</th>
            <th>Payment Date</th>
          </tr>
        </thead>
        <tbody>
          {payments.map((payment, index) => (
            <tr key={index}>
              <td>{payment.amount.toFixed(2)} ₪</td>
              <td>{new Date(payment.paymentDate).toLocaleDateString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default StudentPayments;
