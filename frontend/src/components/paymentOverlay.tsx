import React, { useState } from "react";
import jsPDF from "jspdf";
import { addPayment } from "../services/api";
import type { Payment } from "../models/paymentModel";

interface Props {
  userId: number;
  open: boolean;
  onClose: () => void;
  amount: number;
  onPaymentSuccess: () => void;
}

const PaymentOverlay: React.FC<Props> = ({ open, onClose, amount, onPaymentSuccess, userId }) => {
  const [paymentDone, setPaymentDone] = useState(false);
  const [transactionId] = useState(() => Math.floor(Math.random() * 1000000).toString());

    console.log(amount)
  if (!open) return null;

  const handlePay = async () => {
    try {
      const paymentData: Omit<Payment, "paymentId"> = {
    userId,
    amount, 
    paymentDate: new Date().toISOString()}
      await addPayment(paymentData);
      setPaymentDone(true);
      onPaymentSuccess();
    } catch (error) {
      console.error("שגיאה ברשת:", error);
    }
  };

  const downloadReceiptPdf = () => {
    const pdf = new jsPDF();
    pdf.setFontSize(18);
    pdf.text("Receipt for Lesson Payment", 20, 20);

    pdf.setFontSize(12);
    pdf.text(`Amount: ${amount} ₪`, 20, 40);
    pdf.text(`Transaction ID: ${transactionId}`, 20, 50);
    pdf.text(`Date: ${new Date().toLocaleDateString("he-IL")}`, 20, 60);

    pdf.save(`lesson_receipt_${transactionId}.pdf`);
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-60 backdrop-blur-sm z-[9999] flex items-center justify-center">
      <div className="bg-white rounded-lg shadow-lg w-full max-w-md p-6 relative text-right">
        <button onClick={onClose} className="absolute top-2 left-3 text-xl text-gray-500 hover:text-red-500">✕</button>

        {!paymentDone ? (
          <>
            <h2 className="text-xl font-bold mb-4">Payment for Lesson</h2>
            <p className="mb-4">Amount to Pay: <strong>{amount} ₪</strong></p>

            <button
              onClick={handlePay}
              className="bg-blue-600 text-white py-2 w-full mt-4 rounded hover:bg-blue-700 transition"
            >
              Confirm and Pay
            </button>
          </>
        ) : (
          <div>
            <h2 className="text-xl text-green-600 font-bold mb-4">Payment Successful!</h2>
            <div className="bg-gray-100 p-4 rounded text-sm leading-relaxed">
              <p><strong>Receipt Details:</strong></p>
              <p>• Amount: {amount} ₪</p>
              <p>• Transaction ID: {transactionId}</p>
              <p>• Date: {new Date().toLocaleDateString("he-IL")}</p>
            </div>

            <button
              onClick={downloadReceiptPdf}
              className="mt-4 bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
            >
              Download PDF
            </button>

            <button
              onClick={onClose}
              className="mt-2 bg-gray-300 px-4 py-2 rounded hover:bg-gray-400 block w-full"
            >
              Close
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default PaymentOverlay;
