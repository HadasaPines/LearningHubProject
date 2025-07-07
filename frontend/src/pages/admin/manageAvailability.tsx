import React, { useState, useEffect } from "react";
import type { User } from "../../models/userModel";
import type { TeacherAvailability } from "../../models/availabilityModel";
import {
  getAllTeachers,
  addAvailability,
  getAllAvailabilities,
  // וודא שקיים עדכון זמינות
  updateAvailability,
} from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";

const weekDays = [
  { value: 1, label: "ראשון" },
  { value: 2, label: "שני" },
  { value: 3, label: "שלישי" },
  { value: 4, label: "רביעי" },
  { value: 5, label: "חמישי" },
  { value: 6, label: "שישי" },
  { value: 7, label: "שבת" },
];

const ManageAvailability = () => {
  const [teachers, setTeachers] = useState<User[]>([]);
  const [availabilities, setAvailabilities] = useState<TeacherAvailability[]>([]);

  // מצב להוספה חדשה
  const [newAvailability, setNewAvailability] = useState<Omit<TeacherAvailability, "availabilityId">>({
    teacherId: 0,
    weekDay: 1,
    startTime: "",
    endTime: "",
  });

  // מצב לעריכה
  const [editingAvailabilityId, setEditingAvailabilityId] = useState<number | null>(null);
  const [editAvailability, setEditAvailability] = useState<Omit<TeacherAvailability, "availabilityId">>({
    teacherId: 0,
    weekDay: 1,
    startTime: "",
    endTime: "",
  });

  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const showMessage = (msg: string, type: "success" | "error") => {
    if (type === "success") setSuccessMessage(msg);
    else setErrorMessage(msg);

    setTimeout(() => {
      setSuccessMessage(null);
      setErrorMessage(null);
    }, 3000);
  };

  useEffect(() => {
    (async () => {
      try {
        const [teachersRes, availabilitiesRes] = await Promise.all([getAllTeachers(), getAllAvailabilities()]);
        setTeachers(teachersRes.data);
        setAvailabilities(availabilitiesRes.data);
      } catch (err) {
        showMessage(parseApiError(err), "error");
      }
    })();
  }, []);

  // שינוי בשדות הוספה
  const handleNewChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setNewAvailability((prev) => ({
      ...prev,
      [name]: name === "teacherId" || name === "weekDay" ? +value : value,
    }));
  };

  // שינוי בשדות עריכה
  const handleEditChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setEditAvailability((prev) => ({
      ...prev,
      [name]: name === "teacherId" || name === "weekDay" ? +value : value,
    }));
  };

  const handleAdd = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addAvailability(newAvailability);
      showMessage("זמינות נוספה בהצלחה", "success");
      setNewAvailability({ teacherId: 0, weekDay: 1, startTime: "", endTime: "" });
      const updated = await getAllAvailabilities();
      setAvailabilities(updated.data);
    } catch (err) {
      showMessage(parseApiError(err), "error");
    }
  };


const formatTimeToTimeOnly = (timeString: any) => {
  const [hours, minutes] = timeString.split(":");
  return `${hours}:${minutes}`;
};

  const handleSaveEditAvailability = async () => {
    console.log("Saving edited availability:", formatTimeToTimeOnly( editAvailability.endTime));
    if (!editingAvailabilityId) return;
    try {
      const patch = [
        { op: "replace", path: "/teacherId", value: editAvailability.teacherId },
        { op: "replace", path: "/weekDay", value: editAvailability.weekDay },
        { op: "replace", path: "/startTime", value: formatTimeToTimeOnly(editAvailability.startTime )},
        { op: "replace", path: "/endTime", value:formatTimeToTimeOnly( editAvailability.endTime) },
      ];

      await updateAvailability(editingAvailabilityId, patch);
      showMessage("הזמינות עודכנה בהצלחה", "success");
      setEditingAvailabilityId(null);
      setEditAvailability({ teacherId: 0, weekDay: 1, startTime: "", endTime: "" });
      const updated = await getAllAvailabilities();
      setAvailabilities(updated.data);
    } catch (err) {
      showMessage("שגיאה בעדכון זמינות: " + parseApiError(err), "error");
    }
  };

  // פונקציה לקבלת שם מלא של מורה לפי ID
  const getTeacherName = (teacherId: number) => {
    const teacher = teachers.find((t) => t.userId === teacherId);
    return teacher ? `${teacher.firstName} ${teacher.lastName}` : "לא נמצא מורה";
  };

  return (
    <div>
      <h2>ניהול זמינות מורים</h2>
      {errorMessage && <div style={{ color: "red" }}>{errorMessage}</div>}
      {successMessage && <div style={{ color: "green" }}>{successMessage}</div>}

      <form onSubmit={handleAdd}>
        <h3>הוספת זמינות חדשה</h3>
        <select name="teacherId" value={newAvailability.teacherId} onChange={handleNewChange} required>
          <option value={0}>בחר מורה</option>
          {teachers.map((t) => (
            <option key={t.userId} value={t.userId}>
              {t.firstName} {t.lastName}
            </option>
          ))}
        </select>

        <select name="weekDay" value={newAvailability.weekDay} onChange={handleNewChange}>
          {weekDays.map((d) => (
            <option key={d.value} value={d.value}>
              {d.label}
            </option>
          ))}
        </select>

        <input
          name="startTime"
          type="time"
          value={newAvailability.startTime}
          onChange={handleNewChange}
          required
        />
        <input
          name="endTime"
          type="time"
          value={newAvailability.endTime}
          onChange={handleNewChange}
          required
        />
        <button type="submit">הוסף זמינות</button>
      </form>

      <hr />

      <h3>רשימת זמינויות</h3>
      <table>
        <thead>
          <tr>
            <th>מורה</th>
            <th>יום</th>
            <th>משעה</th>
            <th>עד שעה</th>
            <th>פעולות</th>
          </tr>
        </thead>
        <tbody>
          {availabilities.map((a) =>
            editingAvailabilityId === a.availabilityId ? (
              <tr key={a.availabilityId}>
                <td>
                  <select name="teacherId" value={editAvailability.teacherId} onChange={handleEditChange}>
                    {teachers.map((t) => (
                      <option key={t.userId} value={t.userId}>
                        {t.firstName} {t.lastName}
                      </option>
                    ))}
                  </select>
                </td>
                <td>
                  <select name="weekDay" value={editAvailability.weekDay} onChange={handleEditChange}>
                    {weekDays.map((d) => (
                      <option key={d.value} value={d.value}>
                        {d.label}
                      </option>
                    ))}
                  </select>
                </td>
                <td>
                  <input
                    name="startTime"
                    type="time"
                    value={editAvailability.startTime}
                    onChange={handleEditChange}
                  />
                </td>
                <td>
                  <input
                    name="endTime"
                    type="time"
                    value={editAvailability.endTime}
                    onChange={handleEditChange}
                  />
                </td>
                <td>
                  <button onClick={handleSaveEditAvailability}>💾</button>
                  <button onClick={() => setEditingAvailabilityId(null)}>❌</button>
                </td>
              </tr>
            ) : (
              <tr key={a.availabilityId}>
                <td>{getTeacherName(a.teacherId)}</td>
                <td>{weekDays.find((d) => d.value === a.weekDay)?.label}</td>
                <td>{a.startTime}</td>
                <td>{a.endTime}</td>
                <td>
                  <button
                    onClick={() => {
                      setEditingAvailabilityId(a.availabilityId);
                      setEditAvailability({
                        teacherId: a.teacherId,
                        weekDay: a.weekDay,
                        startTime: a.startTime,
                        endTime: a.endTime,
                      });
                    }}
                  >
                    ✏️
                  </button>
                </td>
              </tr>
            )
          )}
        </tbody>
      </table>
    </div>
  );
};

export default ManageAvailability;
