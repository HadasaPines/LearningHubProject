import React, { useState } from "react";
import jsPDF from "jspdf";

interface Props {
  open: boolean;
  onClose: () => void;
  amount: number;
  onPaymentSuccess: () => void;
}

const PaymentOverlay: React.FC<Props> = ({ open, onClose, amount, onPaymentSuccess }) => {
  const [method, setMethod] = useState("CreditCard");
  const [cardNumber, setCardNumber] = useState("");
  const [expiry, setExpiry] = useState("");
  const [cvv, setCvv] = useState("");
  const [paymentDone, setPaymentDone] = useState(false);
  const [transactionId] = useState(() => Math.floor(Math.random() * 1000000).toString());

  if (!open) return null;

  const handlePay = () => {
    setPaymentDone(true);
    onPaymentSuccess();
  };

  const downloadReceiptPdf = () => {
    const pdf = new jsPDF();
    pdf.setFontSize(18);
    pdf.text("Receipt for Lesson Payment", 20, 20);

    pdf.setFontSize(12);
    pdf.text(`Amount: ${amount} ₪`, 20, 40);
    pdf.text(`Payment Method: ${getMethodLabel(method)}`, 20, 50);
    pdf.text(`Transaction ID: ${transactionId}`, 20, 60);
    pdf.text(`Date: ${new Date().toLocaleDateString("he-IL")}`, 20, 70);

    pdf.save(`lesson_receipt_${transactionId}.pdf`);
  };

  const getMethodLabel = (method: string) => {
    switch (method) {
      case "CreditCard":
        return "Credit Card";
      case "PayPal":
        return "PayPal";
      case "Bit":
        return "Bit";
      default:
        return "Unknown";
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-60 backdrop-blur-sm z-[9999] flex items-center justify-center">
      <div className="bg-white rounded-lg shadow-lg w-full max-w-md p-6 relative text-right">
        <button onClick={onClose} className="absolute top-2 left-3 text-xl text-gray-500 hover:text-red-500">✕</button>

        {!paymentDone ? (
          <>
            <h2 className="text-xl font-bold mb-2">Payment for Lesson</h2>
            <p className="mb-4">Amount to Pay: <strong>{amount} ₪</strong></p>

            <label>Select Payment Method:</label>
            <select value={method} onChange={(e) => setMethod(e.target.value)} className="w-full border p-2 rounded mb-4">
              <option value="CreditCard">Credit Card</option>
              <option value="PayPal">PayPal</option>
              <option value="Bit">Bit</option>
            </select>

            {method === "CreditCard" && (
              <>
                <label>Card Number:</label>
                <input className="border p-2 w-full mb-2" value={cardNumber} onChange={(e) => setCardNumber(e.target.value)} />

                <div className="flex gap-2">
                  <input className="border p-2 w-1/2" placeholder="MM/YY" value={expiry} onChange={(e) => setExpiry(e.target.value)} />
                  <input className="border p-2 w-1/2" placeholder="CVV" value={cvv} onChange={(e) => setCvv(e.target.value)} />
                </div>
              </>
            )}

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
              <p>• Payment Method: {getMethodLabel(method)}</p>
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
