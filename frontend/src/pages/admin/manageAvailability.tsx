
import React, { useState, useEffect } from "react";
import type { User } from "../../models/userModel";
import type { TeacherAvailability } from "../../models/availabilityModel";
import {
  getAllTeachers,
  addAvailability,
  getAllAvailabilities
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
  const [newAvailability, setNewAvailability,] = useState<Omit<TeacherAvailability, "AvailabilityId">>({
    teacherId: 0,
    weekDay: 1,
    startTime: "",
    endTime: "",
  });
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const showMessage = (msg: string, type: "success" | "error") => {
    type === "success" ? setSuccessMessage(msg) : setErrorMessage(msg);
    setTimeout(() => {
      setSuccessMessage(null);
      setErrorMessage(null);
    }, 3000);
  };

  useEffect(() => {
    (async () => {
      try {
        const [teachers, availabilities] = await Promise.all([getAllTeachers(), getAllAvailabilities()]);
        setTeachers(teachers.data);
        setAvailabilities(availabilities.data);
      } catch (err) {
        showMessage(parseApiError(err), "error");
      }
    })();
  }, []);

  const handleNewChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setNewAvailability((prev) => ({
      ...prev,
      [name]: name === "teacherId" || name === "weekDay" ? +value : value,
    }));
  };

  const handleAdd = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addAvailability(newAvailability,);
      showMessage("זמינות נוספה בהצלחה", "success");
      setNewAvailability({ teacherId: 0, weekDay: 1, startTime: "", endTime: "" });
      const updated = await getAllAvailabilities();
      setAvailabilities(updated.data);
    } catch (err) {
      showMessage(parseApiError(err), "error");
    }
  };

  return (
    <div>
      <h2>ניהול זמינות מורים</h2>
      {errorMessage && <div style={{ color: "red" }}>{errorMessage}</div>}
      {successMessage && <div style={{ color: "green" }}>{successMessage}</div>}

      <form onSubmit={handleAdd}>
        <h3>הוספת זמינות</h3>
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
        <input name="startTime" type="time" value={newAvailability.startTime} onChange={handleNewChange} required />
        <input name="endTime" type="time" value={newAvailability.endTime} onChange={handleNewChange} required />
        <button type="submit">הוסף</button>
      </form>

      <hr />
      <h3>רשימת זמינויות</h3>
      <ul>
        {availabilities.map((a) => (
          <li key={a.AvailabilityId}>
            מורה #{a.teacherId}, יום {weekDays.find((d) => d.value === a.weekDay)?.label}, בין {a.startTime} ל־{a.endTime}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ManageAvailability;
