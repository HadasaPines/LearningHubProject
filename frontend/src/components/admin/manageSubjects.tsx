import React, { useEffect, useState } from "react";
import {
  getAllSubjects,
  addSubject,
} from "../../services/api";
import type { Subject } from "../../models/subjectModel";
import { parseApiError } from "../../utils/apiErrorParser";

const SUBJECT_OPTIONS = [
  "Math",
  "English",
  "Grammar",
  "History",
  "Biology",
  "Sciences",
  "Geography",
  "Literature",
];

const ManageSubjects = () => {
  const [subjects, setSubjects] = useState<Subject[]>([]);
  const [newSubject, setNewSubject] = useState<{ name: string }>({ name: "" });

  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  useEffect(() => {
    loadSubjects();
  }, []);

  const loadSubjects = async () => {
    try {
      const res = await getAllSubjects();
      setSubjects(res.data);
    } catch (error) {
      showMessage(parseApiError(error), "error");
    }
  };

  const showMessage = (msg: string, type: "error" | "success") => {
    if (type === "error") setErrorMessage(msg);
    else setSuccessMessage(msg);

    setTimeout(() => {
      setErrorMessage(null);
      setSuccessMessage(null);
    }, 3000);
  };

  const handleAdd = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addSubject(newSubject);
      setNewSubject({ name: "" });
      loadSubjects();
      showMessage("Subject added successfully", "success");
    } catch (error) {
      showMessage(parseApiError(error), "error");
    }
  };

  return (
    <div>
      <h2>Subject Management</h2>

      {errorMessage && <div style={{ color: "red" }}>{errorMessage}</div>}
      {successMessage && <div style={{ color: "green" }}>{successMessage}</div>}

      <form onSubmit={handleAdd}>
        <h3>Add New Subject</h3>
        <select
          value={newSubject.name}
          onChange={(e) => setNewSubject({ name: e.target.value })}
          required
        >
          <option value="">Select Subject</option>
          {SUBJECT_OPTIONS.map((option) => (
            <option key={option} value={option}>
              {option}
            </option>
          ))}
        </select>
        <button type="submit">Add</button>
      </form>

      <hr />

      <h3>Subjects List</h3>
      <table cellPadding={8}>
        <thead>
          <tr>
            <th>Name</th>
          </tr>
        </thead>
        <tbody>
          {subjects.map((s) => (
            <tr key={s.subjectId}>
              <td>{s.name}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ManageSubjects;
