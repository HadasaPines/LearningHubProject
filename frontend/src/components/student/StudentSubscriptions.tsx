import React, { useEffect, useState } from "react";
import type { StudentSubscription } from "../../models/studentSubscriptionModel";
import { getStudentSubscriptionById } from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";
import type {User} from "../../models/userModel"

const StudentSubscriptions: React.FC = () => {

      const user = localStorage.getItem("user");
      if (!user) return <div>User not logged in</div>;
      const parsedUser: User = JSON.parse(user);
  const [studentSubscriptions, setStudentSubscriptions] = useState<StudentSubscription[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  const loadSubscriptions = async () => {
    try {
      const studentSubs = await getStudentSubscriptionById(parsedUser.userId); 
      setStudentSubscriptions(studentSubs.data); 
      setSuccess("Subscriptions loaded successfully.");
    } catch (err) {
      setError("Failed to load subscriptions: " + parseApiError(err));
    }
  };

  useEffect(() => {
    loadSubscriptions();
  }, []);

  return (
    <div>
      <h2 >My Subscriptions</h2>

      {error && <div >{error}</div>}
      {success && <div >{success}</div>}

      <div >
        {studentSubscriptions.length === 0 ? (
          <p>No subscriptions found.</p>
        ) : (
          studentSubscriptions.map((sub) => (
            <div key={sub.studentSubscriptionId}>
              <p><strong>ID:</strong> {sub.subscriptionId}</p>
              <p><strong>Lessons Used:</strong> {sub.lessonsUsed}</p>
              <p><strong>Status:</strong> {sub.isActive ? "Active" : "Inactive"}</p>
            </div>
          ))
        )}
      </div>
    </div>
  );
};

export default StudentSubscriptions;
