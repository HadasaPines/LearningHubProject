import React, { useState, useEffect } from "react";
import type { User } from "../../models/userModel";
import type { TeacherAvailability } from "../../models/availabilityModel";
import {
  getAllTeachers,
  addAvailability,
  getAllAvailabilities,
  updateAvailability,
  deleteAvailability,
} from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";

const weekDays = [
  { value: 1, label: "Sunday" },
  { value: 2, label: "Monday" },
  { value: 3, label: "Tuesday" },
  { value: 4, label: "Wednesday" },
  { value: 5, label: "Thursday" },
  { value: 6, label: "Friday" },
  { value: 7, label: "Saturday" },
];

const ManageAvailability = () => {
  const [teachers, setTeachers] = useState<User[]>([]);
  const [availabilities, setAvailabilities] = useState<TeacherAvailability[]>([]);

  // State for new availability
  const [newAvailability, setNewAvailability] = useState<Omit<TeacherAvailability, "availabilityId">>({
    teacherId: 0,
    subjectId: 0,
    weekDay: 1,
    minAge: null,
    maxAge: null,
    startTime: "",
    endTime: "",
  });

  const [editingAvailabilityId, setEditingAvailabilityId] = useState<number | null>(null);
  const [editAvailability, setEditAvailability] = useState<Omit<TeacherAvailability, "availabilityId">>({
    teacherId: 0,
    subjectId: 0,
    weekDay: 1,
    minAge: null,
    maxAge: null,
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

  const handleNewChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setNewAvailability((prev) => {
      if (name === "teacherId" || name === "weekDay" || name === "subjectId") {
        return { ...prev, [name]: +value };
      }
      if (name === "minAge" || name === "maxAge") {
        return { ...prev, [name]: value === "" ? null : +value };
      }
      return { ...prev, [name]: value };
    });
  };

  const handleEditChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setEditAvailability((prev) => {
      if (name === "teacherId" || name === "weekDay" || name === "subjectId") {
        return { ...prev, [name]: +value };
      }
      if (name === "minAge" || name === "maxAge") {
        return { ...prev, [name]: value === "" ? null : +value };
      }
      return { ...prev, [name]: value };
    });
  };

  const handleAdd = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addAvailability(newAvailability);
      showMessage("Availability added successfully", "success");
      setNewAvailability({
        teacherId: 0,
        subjectId: 0,
        weekDay: 1,
        minAge: null,
        maxAge: null,
        startTime: "",
        endTime: "",
      });
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
    if (!editingAvailabilityId) return;
    try {
      const patch = [
        { op: "replace", path: "/teacherId", value: editAvailability.teacherId },
        { op: "replace", path: "/subjectId", value: editAvailability.subjectId },
        { op: "replace", path: "/weekDay", value: editAvailability.weekDay },
        { op: "replace", path: "/minAge", value: editAvailability.minAge },
        { op: "replace", path: "/maxAge", value: editAvailability.maxAge },
        { op: "replace", path: "/startTime", value: formatTimeToTimeOnly(editAvailability.startTime) },
        { op: "replace", path: "/endTime", value: formatTimeToTimeOnly(editAvailability.endTime) },
      ];

      await updateAvailability(editingAvailabilityId, patch);
      showMessage("Availability updated successfully", "success");
      setEditingAvailabilityId(null);
      setEditAvailability({
        teacherId: 0,
        subjectId: 0,
        weekDay: 1,
        minAge: null,
        maxAge: null,
        startTime: "",
        endTime: "",
      });
      const updated = await getAllAvailabilities();
      setAvailabilities(updated.data);
    } catch (err) {
      showMessage("Error updating availability: " + parseApiError(err), "error");
    }
  };

  const getTeacherName = (teacherId: number) => {
    const teacher = teachers.find((t) => t.userId === teacherId);
    return teacher ? `${teacher.firstName} ${teacher.lastName}` : "Teacher not found";
  };

  const handleDeleteLesson = async (availabilityId: number) => {
    if (window.confirm("Are you sure you want to delete this availability? All lessons for this availability will also be deleted...")) {
      try {
        await deleteAvailability(availabilityId);
        setSuccessMessage("Deleted successfully");
        const updated = await getAllAvailabilities();
        setAvailabilities(updated.data);
      } catch (error) {
        setErrorMessage("Error deleting availability: " + parseApiError(error));
      }
    }
  };

  return (
    <div>
      <h2>Manage Teacher Availability</h2>
      {errorMessage && <div style={{ color: "red" }}>{errorMessage}</div>}
      {successMessage && <div style={{ color: "green" }}>{successMessage}</div>}

      <form onSubmit={handleAdd}>
        <h3>Add New Availability</h3>
        <select name="teacherId" value={newAvailability.teacherId} onChange={handleNewChange} required>
          <option value={0}>Select Teacher</option>
          {teachers.map((t) => (
            <option key={t.userId} value={t.userId}>
              {t.firstName} {t.lastName}
            </option>
          ))}
        </select>

        <select name="subjectId" value={newAvailability.subjectId} onChange={handleNewChange} required>
          <option value={0}>Select Subject</option>
          {/* TODO: הכנס כאן אפשרויות מקצועות */}
        </select>

        <select name="weekDay" value={newAvailability.weekDay} onChange={handleNewChange}>
          {weekDays.map((d) => (
            <option key={d.value} value={d.value}>
              {d.label}
            </option>
          ))}
        </select>

        <input
          name="minAge"
          type="number"
          value={newAvailability.minAge ?? ""}
          onChange={handleNewChange}
          placeholder="Min Age"
          min={0}
          style={{ width: "80px", marginRight: "8px" }}
        />
        <input
          name="maxAge"
          type="number"
          value={newAvailability.maxAge ?? ""}
          onChange={handleNewChange}
          placeholder="Max Age"
          min={0}
          style={{ width: "80px" }}
        />

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
        <button type="submit">Add Availability</button>
      </form>

      <hr />

      <h3>Availability List</h3>
      <table>
        <thead>
          <tr>
            <th>Teacher</th>
            <th>Subject</th>
            <th>Day</th>
            <th>Min Age</th>
            <th>Max Age</th>
            <th>From</th>
            <th>To</th>
            <th>Actions</th>
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
                  <select name="subjectId" value={editAvailability.subjectId} onChange={handleEditChange} required>
                    <option value={0}>Select Subject</option>
                    {/* TODO: הכנס כאן אפשרויות מקצועות */}
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
                    name="minAge"
                    type="number"
                    value={editAvailability.minAge ?? ""}
                    onChange={handleEditChange}
                    placeholder="Min Age"
                    min={0}
                    style={{ width: "80px" }}
                  />
                </td>
                <td>
                  <input
                    name="maxAge"
                    type="number"
                    value={editAvailability.maxAge ?? ""}
                    onChange={handleEditChange}
                    placeholder="Max Age"
                    min={0}
                    style={{ width: "80px" }}
                  />
                </td>
                <td>
                  <input
                    name="startTime"
                    type="time"
                    value={editAvailability.startTime}
                    onChange={handleEditChange}
                    required
                  />
                </td>
                <td>
                  <input
                    name="endTime"
                    type="time"
                    value={editAvailability.endTime}
                    onChange={handleEditChange}
                    required
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
                <td>{/* TODO: להציג את שם המקצוע לפי a.subjectId */}</td>
                <td>{weekDays.find((d) => d.value === a.weekDay)?.label}</td>
                <td>{a.minAge ?? "-"}</td>
                <td>{a.maxAge ?? "-"}</td>
                <td>{a.startTime}</td>
                <td>{a.endTime}</td>
                <td>
                  <button
                    onClick={() => {
                      setEditingAvailabilityId(a.availabilityId);
                      setEditAvailability({
                        teacherId: a.teacherId,
                        subjectId: a.subjectId,
                        weekDay: a.weekDay,
                        minAge: a.minAge ?? null,
                        maxAge: a.maxAge ?? null,
                        startTime: a.startTime,
                        endTime: a.endTime,
                      });
                    }}
                  >
                    ✏️
                  </button>

                  <button
                    onClick={() => handleDeleteLesson(a.availabilityId)}
                    style={{
                      border: "none",
                      padding: "5px 10px",
                    }}
                  >
                    🗑️
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
