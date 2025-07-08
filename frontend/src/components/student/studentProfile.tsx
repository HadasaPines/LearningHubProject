import React, { useState } from "react";
import type { User } from "../../models/userModel";
import { updateUser, updateStudent } from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";


const StudentProfile: React.FC = () => {
  const user = localStorage.getItem("user");
  if (!user) return <div>משתמש לא מחובר</div>;
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
           showMessage("העדכון נשמר בהצלחה", "success");
         } catch (error: any) {
           showMessage(parseApiError(error), "error");
         }
  

      setIsEditing(false);
    } catch (err) {
    showMessage("שגיאה בשמירת הנתונים", "error");
    }
  };

  return (
    <div dir="rtl">
      <h2>פרטי תלמיד</h2>
 {errorMessage && <div style={{ color: "red" }}>{errorMessage}</div>}
      {successMessage && <div style={{ color: "green" }}>{successMessage}</div>}
      {!isEditing ? (
        <>
          <p><strong>שם:</strong> {userData.firstName} {userData.lastName}</p>
          <p><strong>אימייל:</strong> {userData.email}</p>
          <p><strong>טלפון:</strong> {userData.phone}</p>
          <p><strong>תאריך לידה:</strong> {userData.student?.birthDate}</p>
          <p><strong>גיל:</strong> {userData.student?.age}</p>
          <p><strong>מין:</strong> {userData.student?.gender === "F" ? "נקבה" : "זכר"}</p>
          <button onClick={() => setIsEditing(true)}>ערוך</button>
        </>
      ) : (
        <>
          <label>
            שם פרטי:
            <input
              name="firstName"
              value={userData.firstName}
              onChange={handleChange}
            />
          </label>
          <br />
          <label>
            שם משפחה:
            <input
              name="lastName"
              value={userData.lastName}
              onChange={handleChange}
            />
          </label>
          <br />
          <label>
            אימייל:
            <input
              name="email"
              value={userData.email}
              onChange={handleChange}
            />
          </label>
          <br />
          <label>
            טלפון:
            <input
              name="phone"
              value={userData.phone}
              onChange={handleChange}
            />
          </label>
          <br />

          <br />
          <label>
            גיל:
            <input
              name="age"
              value={userData.student?.age || ""}
              onChange={handleChange}
            />
          </label>
          <br />

          <br />
          <button onClick={handleSave}>שמור</button>
          <button onClick={() => setIsEditing(false)}>ביטול</button>
        </>
      )}
    </div>
  );

};
export default StudentProfile;
