import React, { useState, useEffect } from "react";
import {
  addSubscription,
  getAllSubscriptions,
  updateSubscriptionActive,
} from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";
import type { Subscription } from "../../models/subscriptionModel";
import styles from "./ManageSubscriptions.module.scss";
import { FaRegIdCard } from "react-icons/fa";
import { FiEdit2 } from "react-icons/fi";


const ManageSubscriptions = () => {
  const [newSubscription, setNewSubscription] = useState<
    Omit<Subscription, "subscriptionId">
  >({
    name: "",
    description: "",
    price: 0,
    lessonCount: null,
    isActive: true,
  });

  const [subscriptions, setSubscriptions] = useState<Subscription[]>([]);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);

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
    const { name, value, type } = e.target;

    if (type === "checkbox") {
      const checked = (e.target as HTMLInputElement).checked;
      setNewSubscription((prev) => ({ ...prev, [name]: checked }));
    } else if (["price", "lessonCount"].includes(name)) {
      const val = value === "" ? null : Number(value);
      setNewSubscription((prev) => ({ ...prev, [name]: val }));
    } else {
      setNewSubscription((prev) => ({ ...prev, [name]: value }));
    }
  };

  const handleAdd = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newSubscription.name || newSubscription.price <= 0) {
      showMessage(
        "Name and Price are required, and Price must be positive",
        "error"
      );
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
        isActive: true,
      });
      setShowForm(false);

      const updated = await getAllSubscriptions();
      setSubscriptions(updated.data);
    } catch (error) {
      showMessage("Error adding subscription: " + parseApiError(error), "error");
    }
  };

  const handleToggleActive = async (subscription: Subscription) => {
    try {
      const updatedSubscription = {
        ...subscription,
        isActive: !subscription.isActive,
      };
      await updateSubscriptionActive(subscription.subscriptionId!);
      showMessage(
        `Subscription "${subscription.name}" is now ${updatedSubscription.isActive ? "active" : "inactive"
        }`,
        "success"
      );

      const refreshed = await getAllSubscriptions();
      setSubscriptions(refreshed.data);
    } catch (error) {
      showMessage("Error updating subscription: " + parseApiError(error), "error");
    }
  };

  useEffect(() => {
    const fetchSubscriptions = async () => {
      try {
        const response = await getAllSubscriptions();
        setSubscriptions(response.data);
      } catch (error) {
        showMessage(
          "Error loading subscriptions: " + parseApiError(error),
          "error"
        );
      }
    };

    fetchSubscriptions();
  }, []);

  return (
    <div className={styles.container}>
      <h2 className={styles.title}>Manage Subscriptions</h2>

      {errorMessage && <div style={{ color: "red" }}>{errorMessage}</div>}
      {successMessage && <div style={{ color: "green" }}>{successMessage}</div>}

      {!showForm && (
        <button className={styles.addBtn} onClick={() => setShowForm(true)}>
          + Add New Subscription
        </button>
      )}

      {showForm && (
        <div className={styles.modalOverlay}>
          <div className={styles.modalBox}>
            <h3>Add New Subscription</h3>
            <form className={styles.form} onSubmit={handleAdd}>
              <input
                type="text"
                name="name"
                placeholder="Name"
                value={newSubscription.name}
                onChange={handleChange}
                required
              />
              <textarea
                name="description"
                placeholder="Description"
                value={newSubscription.description}
                onChange={handleChange}
              />
              <input
                type="number"
                name="price"
                placeholder="Price"
                value={newSubscription.price || ""}
                onChange={handleChange}
                min={0}
                step="0.01"
                required
              />
              <input
                type="number"
                name="lessonCount"
                placeholder="Lesson Count"
                value={newSubscription.lessonCount ?? ""}
                onChange={handleChange}
                min={0}
                required
              />
      

              <div className={styles.actions}>
                <button type="submit">Save</button>
                <button type="button" onClick={() => setShowForm(false)}>
                  Cancel
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      <h3>Existing Subscriptions</h3>
      {subscriptions.length === 0 ? (
        <p>No subscriptions available.</p>
      ) : (
        <div className={styles.cardGrid}>
          {subscriptions.map((sub) => (
            <div key={sub.subscriptionId} className={styles.card}>
              <div className={styles.icon}><FaRegIdCard /></div>
              <h4>{sub.name}</h4>
              <div className={styles.content}>
                <p>{sub.description || "-"}</p>
                <p>Price: ₪{sub.price.toFixed(2)}</p>
                <p>Lessons: {sub.lessonCount ?? "-"}</p>
                <p>Status: {sub.isActive ? "Active" : "Inactive"}</p>
              </div>
              <div className={styles.actions}>
                <button onClick={() => handleToggleActive(sub)}>
                  {sub.isActive ? "Deactivate" : "Activate"} <FiEdit2 />
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default ManageSubscriptions;
