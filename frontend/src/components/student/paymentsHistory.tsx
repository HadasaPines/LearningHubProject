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
    <div className="p-4">
      <h2 className="text-xl font-bold mb-4">Payment History</h2>
      <table className="table-auto w-full border">
        <thead className="bg-gray-100">
          <tr>
            <th className="border px-4 py-2">Amount</th>
            <th className="border px-4 py-2">Payment Date</th>
          </tr>
        </thead>
        <tbody>
          {payments.map((payment, index) => (
            <tr key={index}>
              <td className="border px-4 py-2">{payment.amount.toFixed(2)} ₪</td>
              <td className="border px-4 py-2">
                {new Date(payment.paymentDate).toLocaleDateString()}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default StudentPayments;
