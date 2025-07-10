export interface Subscription {
  subscriptionId?: number;
  name: string;
  description?: string;
  price: number;
  lessonCount?: number | null;
  validityDays?: number | null;
}