import React, { useState } from "react";
import type { User } from "../../models/userModel";
import { updateUser, updateStudent } from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";

const StudentProfile: React.FC = () => {
  const user = localStorage.getItem("user");
  if (!user) return <div>User not logged in</div>;
  const parsedUser: User = JSON.parse(user);

  const [isEditing, setIsEditing] = useState(false);
  const [userData, setUserData] = useState<User>(parsedUser);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null)

  const showMessage = (msg: string, type: "error" | "success") => {
    if (type === "error") setErrorMessage(msg);
    else setSuccessMessage(msg);

    setTimeout(() => {
      setErrorMessage(null);
      setSuccessMessage(null);
    }, 4000);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    const studentField = ["age"];
    if (studentField.includes(name)) {
      setUserData({
        ...userData,
        student: { ...userData.student!, [name]: value }
      });
    } else {
      setUserData({
        ...userData,
        [name]: value
      });
    }
  };

  const handleSave = async () => {
    try {
      localStorage.setItem("user", JSON.stringify(userData));

      try {
        const userPatch = [
          { op: "replace", path: "/firstName", value: parsedUser.firstName },
          { op: "replace", path: "/lastName", value: parsedUser.lastName },
          { op: "replace", path: "/email", value: parsedUser.email },
          { op: "replace", path: "/phone", value: parsedUser.phone },
        ];

        const studentPatch = [
          { op: "replace", path: "/age", value: parsedUser.student?.age },
        ];

        await updateUser(parsedUser.userId, userPatch);
        await updateStudent(parsedUser.userId, studentPatch);
        showMessage("Update saved successfully", "success");
      } catch (error: any) {
        showMessage(parseApiError(error), "error");
      }

      setIsEditing(false);
    } catch (err) {
      showMessage("Error saving data", "error");
    }
  };

  return (
    <div dir="rtl">
      <h2>Student Details</h2>
      {errorMessage && <div style={{ color: "red" }}>{errorMessage}</div>}
      {successMessage && <div style={{ color: "green" }}>{successMessage}</div>}
      {!isEditing ? (
        <>
          <p><strong>Name:</strong> {userData.firstName} {userData.lastName}</p>
          <p><strong>Email:</strong> {userData.email}</p>
          <p><strong>Phone:</strong> {userData.phone}</p>
          <p><strong>Birth Date:</strong> {userData.student?.birthDate}</p>
          <p><strong>Age:</strong> {userData.student?.age}</p>
          <p><strong>Gender:</strong> {userData.student?.gender === "F" ? "Female" : "Male"}</p>
          <button onClick={() => setIsEditing(true)}>Edit</button>
        </>
      ) : (
        <>
          <label>
            First Name:
            <input
              name="firstName"
              value={userData.firstName}
              onChange={handleChange}
            />
          </label>
          <br />
          <label>
            Last Name:
            <input
              name="lastName"
              value={userData.lastName}
              onChange={handleChange}
            />
          </label>
          <br />
          <label>
            Email:
            <input
              name="email"
              value={userData.email}
              onChange={handleChange}
            />
          </label>
          <br />
          <label>
            Phone:
            <input
              name="phone"
              value={userData.phone}
              onChange={handleChange}
            />
          </label>
          <br />
          <br />
          <label>
            Age:
            <input
              name="age"
              value={userData.student?.age || ""}
              onChange={handleChange}
            />
          </label>
          <br />
          <br />
          <button onClick={handleSave}>Save</button>
          <button onClick={() => setIsEditing(false)}>Cancel</button>
        </>
      )}
    </div>
  );
};

export default StudentProfile;
