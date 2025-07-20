import React, { useState, useEffect } from "react";
import { FiUser, FiEdit2 } from "react-icons/fi"; 
import type { User } from "../../models/userModel";
import { updateUser, updateStudent } from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";
import Toast from "../../components/toast"

import styles from "./StudentProfile.module.scss";

const StudentProfile: React.FC = () => {
  const user = localStorage.getItem("user");
  if (!user) return <div>User not logged in</div>;
  const parsedUser: User = JSON.parse(user);

  const [isEditing, setIsEditing] = useState(false);
  const [userData, setUserData] = useState<User>(parsedUser);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

 useEffect(() => {
  if (errorMessage || successMessage) {

    const timeout = setTimeout(() => {
      setErrorMessage(null);
      setSuccessMessage(null);
    }, 4000);

    return () => clearTimeout(timeout);
  }
}, [errorMessage, successMessage]);


  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    const studentField = ["age"];
    if (studentField.includes(name)) {
      setUserData((prev) => ({
        ...prev,
        student: { ...prev.student!, [name]: value },
      }));
    } else {
      setUserData((prev) => ({
        ...prev,
        [name]: value,
      }));
    }
  };

  const handleSave = async () => {
    try {
      localStorage.setItem("user", JSON.stringify(userData));
      try {
        const userPatch = [
          { op: "replace", path: "/firstName", value: userData.firstName },
          { op: "replace", path: "/lastName", value: userData.lastName },
          { op: "replace", path: "/email", value: userData.email },
          { op: "replace", path: "/phone", value: userData.phone },
        ];
        const studentPatch = [
          { op: "replace", path: "/age", value: userData.student?.age },
        ];
        await updateUser(parsedUser.userId, userPatch);
        await updateStudent(parsedUser.userId, studentPatch);
        setSuccessMessage("Update saved successfully");
      } catch (error: any) {
        setErrorMessage(parseApiError(error));
      }
      setIsEditing(false);
    } catch {
      setErrorMessage("Error saving data");
    }
  };

  return (
    <>
       {errorMessage && <Toast type="error" message={errorMessage} />}
      {successMessage && <Toast type="success" message={successMessage} />}
    <div className={styles.container}>
      <div className={styles.circle1}></div>
      <div className={styles.circle2}></div>
      <div className={styles.circle3}></div>
      <div className={styles.circle4}></div>




      <div className={styles.card}>
        <div className={styles.profileSection}>
          <div className={styles.profileImage}>
            <FiUser className={styles.userIcon} />
            {!isEditing && (
              <button
                className={styles.editIconButton}
                onClick={() => setIsEditing(true)}
                aria-label="Edit profile"
                type="button"
              >
                <FiEdit2 />
              </button>
            )}
            <div className={styles.userName}>
              {userData.firstName} {userData.lastName}
            </div>
          </div>

          <div className={styles.detailsSection}>
            <h2 className={styles.title}>Student Details</h2>

            {!isEditing ? (
              <>
                <p>
                  <strong>Name:</strong> {userData.firstName} {userData.lastName}
                </p>
                <p>
                  <strong>Email:</strong> {userData.email}
                </p>
                <p>
                  <strong>Phone:</strong> {userData.phone}
                </p>
                <p>
                  <strong>Birth Date:</strong> {userData.student?.birthDate}
                </p>
                <p>
                  <strong>Age:</strong> {userData.student?.age}
                </p>
                <p>
                  <strong>Gender:</strong>{" "}
                  {userData.student?.gender === "F" ? "Female" : "Male"}
                </p>
              </>
            ) : (
              <>
                <label className={styles.label}>
                  First Name:
                  <input
                    className={styles.input}
                    name="firstName"
                    value={userData.firstName}
                    onChange={handleChange}
                  />
                </label>
                <label className={styles.label}>
                  Last Name:
                  <input
                    className={styles.input}
                    name="lastName"
                    value={userData.lastName}
                    onChange={handleChange}
                  />
                </label>
                <label className={styles.label}>
                  Email:
                  <input
                    className={styles.input}
                    name="email"
                    value={userData.email}
                    onChange={handleChange}
                  />
                </label>
                <label className={styles.label}>
                  Phone:
                  <input
                    className={styles.input}
                    name="phone"
                    value={userData.phone}
                    onChange={handleChange}
                  />
                </label>

                <div className={styles.buttonRow}>
                  <button className={styles.confirmButton} onClick={handleSave}>
                    ✔
                  </button>
                  <button
                    className={styles.cancelButton}
                    onClick={() => setIsEditing(false)}
                  >
                    ✕
                  </button>
                </div>
              </>
            )}
          </div>
        </div>
      </div>
    
    </div>
     </>
  );
};

export default StudentProfile;
