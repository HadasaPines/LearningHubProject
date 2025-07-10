import React, { useState, useEffect } from "react";
import { addSubscription, getAllSubscriptions } from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";
import type { Subscription } from "../../models/subscriptionModel";

const ManageSubscriptions = () => {
  const [newSubscription, setNewSubscription] = useState<Omit<Subscription, "subscriptionId">>({
    name: "",
    description: "",
    price: 0,
    lessonCount: null,
    validityDays: null,
  });

  const [subscriptions, setSubscriptions] = useState<Subscription[]>([]);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const showMessage = (msg: string, type: "success" | "error") => {
    type === "success" ? setSuccessMessage(msg) : setErrorMessage(msg);
    setTimeout(() => {
      setSuccessMessage(null);
      setErrorMessage(null);
    }, 3000);
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;

    if (["price", "lessonCount", "validityDays"].includes(name)) {
      const val = value === "" ? null : Number(value);
      setNewSubscription((prev) => ({ ...prev, [name]: val }));
    } else {
      setNewSubscription((prev) => ({ ...prev, [name]: value }));
    }
  };

  const handleAdd = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newSubscription.name || newSubscription.price <= 0) {
      showMessage("Name and Price are required, and Price must be positive", "error");
      return;
    }

    try {
      await addSubscription(newSubscription);
      showMessage("Subscription added successfully", "success");
      setNewSubscription({
        name: "",
        description: "",
        price: 0,
        lessonCount: null,
        validityDays: null,
      });

      // עדכון הרשימה אחרי ההוספה
      const updated = await getAllSubscriptions();
      setSubscriptions(updated.data);
    } catch (error) {
      showMessage("Error adding subscription: " + parseApiError(error), "error");
    }
  };

  useEffect(() => {
    const fetchSubscriptions = async () => {
      try {
        const response = await getAllSubscriptions();
        setSubscriptions(response.data);
      } catch (error) {
        showMessage("Error loading subscriptions: " + parseApiError(error), "error");
      }
    };

    fetchSubscriptions();
  }, []);

  return (
    <div>
      <h2>Add New Subscription</h2>
      {errorMessage && <div style={{ color: "red" }}>{errorMessage}</div>}
      {successMessage && <div style={{ color: "green" }}>{successMessage}</div>}

      <form onSubmit={handleAdd}>
        <div>
          <label>Name*</label>
          <input
            type="text"
            name="name"
            value={newSubscription.name}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label>Description</label>
          <textarea
            name="description"
            value={newSubscription.description}
            onChange={handleChange}
          />
        </div>
        <div>
          <label>Price*</label>
          <input
            type="number"
            name="price"
            value={newSubscription.price ?? ""}
            onChange={handleChange}
            min={0}
            step="0.01"
            required
          />
        </div>
        <div>
          <label>Lesson Count</label>
          <input
            type="number"
            name="lessonCount"
            value={newSubscription.lessonCount ?? ""}
            onChange={handleChange}
            min={0}
          />
        </div>
        <div>
          <label>Validity (days)</label>
          <input
            type="number"
            name="validityDays"
            value={newSubscription.validityDays ?? ""}
            onChange={handleChange}
            min={0}
          />
        </div>

        <button type="submit">Add Subscription</button>
      </form>

      <hr />
      <h3>Existing Subscriptions</h3>
      {subscriptions.length === 0 ? (
        <p>No subscriptions available.</p>
      ) : (
        <table border={1} cellPadding={5}>
          <thead>
            <tr>
              <th>Name</th>
              <th>Description</th>
              <th>Price</th>
              <th>Lessons</th>
              <th>Validity (days)</th>
            </tr>
          </thead>
          <tbody>
            {subscriptions.map((sub) => (
              <tr key={sub.subscriptionId}>
                <td>{sub.name}</td>
                <td>{sub.description}</td>
                <td>{sub.price.toFixed(2)}</td>
                <td>{sub.lessonCount ?? "-"}</td>
                <td>{sub.validityDays ?? "-"}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default ManageSubscriptions;
