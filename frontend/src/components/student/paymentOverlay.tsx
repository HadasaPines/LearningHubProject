import React, { useEffect, useState } from "react";
import jsPDF from "jspdf";
import { addPayment } from "../../services/api";
import type { Payment } from "../../models/paymentModel";
import "./paymentOverlay.scss";
import Toast from "../../components/toast"

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

  const [cardName, setCardName] = useState("");
  const [cardNumber, setCardNumber] = useState("");
  const [expiry, setExpiry] = useState("");
  const [cvv, setCvv] = useState("");

  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  useEffect(() => {
    if (error || success) {
      const timeout = setTimeout(() => {
        setError(null);
        setSuccess(null);
      }, 4000);
      return () => clearTimeout(timeout);
    }
  }, [error, success]);

  if (!open) return null;

  const handlePay = async () => {
    if (!cardName || !cardNumber || !expiry || !cvv) {
      setError("Please fill out all credit card fields.");
      setSuccess(null);
      return;
    }

    try {
      const paymentData: Omit<Payment, "paymentId"> = {
        userId,
        amount,
        paymentDate: new Date().toISOString(),
      };
      await addPayment(paymentData);
      setPaymentDone(true);
      setSuccess("Payment was successful.");
      setError(null);
    } catch (err) {
      setError("An error occurred while processing the payment.");
      setSuccess(null);
    }
  };

  const downloadReceiptPdf = () => {
    const pdf = new jsPDF();
    pdf.setFontSize(18);
    pdf.text("Receipt for Subscription Payment", 20, 20);
    pdf.setFontSize(12);
    pdf.text(`Amount: ${amount} ₪`, 20, 40);
    pdf.text(`Transaction ID: ${transactionId}`, 20, 50);
    pdf.text(`Date: ${new Date().toLocaleDateString("he-IL")}`, 20, 60);
    pdf.save(`subscription_receipt_${transactionId}.pdf`);
    handleCloseAfterSuccess();
  };

  const handleCloseAfterSuccess = () => {
    if(paymentDone){
      onPaymentSuccess();
      onClose();
      resetForm();
    } else {
      onClose();
    }
  };

  const resetForm = () => {
    setPaymentDone(false);
    setCardName("");
    setCardNumber("");
    setExpiry("");
    setCvv("");
    setError(null);
    setSuccess(null);
  };

  return (
    <div className="payment-overlay">
      <div className="payment-card">
        <button onClick={handleCloseAfterSuccess} className="close-button">✕</button>

        {error && <Toast type="error" message={error} />}
        {success && <Toast type="success" message={success} />}

        {!paymentDone ? (
          <>
            <h2 className="payment-title">Payment for Subscription</h2>
            <p className="payment-amount">Amount to Pay: <strong>{amount} ₪</strong></p>

            <div className="credit-form">
              <label>
                Cardholder Name
                <input
                  type="text"
                  value={cardName}
                  onChange={(e) => setCardName(e.target.value)}
                />
              </label>

              <label>
                Card Number
                <input
                  type="text"
                  value={cardNumber}
                  onChange={(e) => setCardNumber(e.target.value)}
                />
              </label>

              <div className="credit-form-row">
                <label>
                  Expiry
                  <input
                    type="text"
                    placeholder="MM/YY"
                    value={expiry}
                    onChange={(e) => setExpiry(e.target.value)}
                  />
                </label>

                <label>
                  CVV
                  <input
                    type="text"
                    value={cvv}
                    onChange={(e) => setCvv(e.target.value)}
                  />
                </label>
              </div>
            </div>

            <button onClick={handlePay} className="confirm-button">
              Confirm and Pay
            </button>
          </>
        ) : (
          <>
            <h2 className="payment-title">Payment Successful!</h2>

            <div className="credit-form">
              <label>
                Amount Paid
                <div className="readonly-field">{amount} ₪</div>
              </label>

              <label>
                Transaction ID
                <div className="readonly-field">{transactionId}</div>
              </label>

              <label>
                Date
                <div className="readonly-field">{new Date().toLocaleDateString("he-IL")}</div>
              </label>
            </div>

            <button onClick={downloadReceiptPdf} className="confirm-button">
              Download PDF
            </button>
          </>
        )}
      </div>
    </div>
  );
};

export default PaymentOverlay;
