export interface PaymentDetails {
  amount: number;
  method: "CreditCard" | "PayPal" | "Bit";
  lessonId: number;
  studentId: number;
}
