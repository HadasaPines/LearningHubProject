import React, { useEffect, useState } from "react";
import styles from "./manageTeachers.module.scss";
import {
  getAllTeachers,
  addUser,
  addTeacher,
  updateUser,
  updateTeacher,
  deleteUser,
  deleteTeacher,
  getLessonsByTeacherId,
} from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";
import type { User } from "../../models/userModel";
import Toast from "../../components/toast";
import { FaEdit, FaTrash, FaSave, FaTimes, FaUserPlus } from "react-icons/fa";
import { FiUser } from "react-icons/fi";

const formatDate = (dateStr: string | undefined): string => {
  if (!dateStr) return "-";
  const date = new Date(dateStr);
  const day = String(date.getDate()).padStart(2, "0");
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const year = date.getFullYear();
  return `${day}/${month}/${year}`;
};

const ManageTeachers = () => {
  const [teachers, setTeachers] = useState<User[]>([]);
  const [newTeacherVisible, setNewTeacherVisible] = useState(false);
  const [newTeacher, setNewTeacher] = useState<Omit<User, "teacherId">>({
    userId: 0,
    firstName: "",
    lastName: "",
    password: "",
    phone: "",
    email: "",
    role: "Teacher",
    teacher: { gender: "", bio: "", birthDate: "" },
  });
  const [editingTeacherId, setEditingTeacherId] = useState<number | null>(null);
  const [editTeacher, setEditTeacher] = useState<User | null>(null);
  const [toast, setToast] = useState<{ type: "success" | "error"; message: string } | null>(null);
  const [birthDateInputType, setBirthDateInputType] = useState<"text" | "date">("text");
  const [expandedTeacherId, setExpandedTeacherId] = useState<number | null>(null);

  // חדש - ניהול כמות שורות ביוגרפיה לכל מורה
  const [expandedBioLines, setExpandedBioLines] = useState<Record<number, number>>({});

  const CHARS_PER_LINE = 25;
  const LINES_DEFAULT = 1;

  const showMessage = (msg: string, type: "success" | "error") => {
    setToast({ message: msg, type });
    setTimeout(() => setToast(null), 3000);
  };

  useEffect(() => {
    (async () => {
      try {
        const res = await getAllTeachers();
        setTeachers(res.data);
      } catch (err) {
        showMessage(parseApiError(err), "error");
      }
    })();
  }, []);

  const toggleDetails = (id: number) => {
    setExpandedTeacherId(prev => (prev === id ? null : id));
  };

  const toggleBioLines = (userId: number) => {
    setExpandedBioLines(prev => {
      const currentLines = prev[userId] || LINES_DEFAULT;
      return {
        ...prev,
        [userId]: currentLines === LINES_DEFAULT ? 3 : LINES_DEFAULT,
      };
    });
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    const teacherFields = ["gender", "bio", "birthDate"];
    if (teacherFields.includes(name)) {
      setNewTeacher(prev => ({
        ...prev,
        teacher: { ...prev.teacher!, [name]: value },
      }));
    } else {
      setNewTeacher(prev => ({ ...prev, [name]: value }));
    }
  };

  const handleAdd = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addUser(newTeacher);
      await addTeacher({
        teacherId: newTeacher.userId,
        gender: newTeacher.teacher!.gender,
        bio: newTeacher.teacher!.bio,
        birthDate: newTeacher.teacher!.birthDate,
      });
      showMessage("Teacher added successfully", "success");
      setNewTeacherVisible(false);
      setNewTeacher({
        userId: 0,
        firstName: "",
        lastName: "",
        password: "",
        phone: "",
        email: "",
        role: "Teacher",
        teacher: { gender: "", bio: "", birthDate: "" },
      });
      const res = await getAllTeachers();
      setTeachers(res.data);
    } catch (err) {
      showMessage(parseApiError(err), "error");
    }
  };

  const handleEditClick = (teacher: User) => {
    setEditingTeacherId(teacher.userId);
    setEditTeacher({ ...teacher });
  };

  const handleEditChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    if (!editTeacher) return;
    const { name, value } = e.target;
    const teacherFields = ["gender", "bio", "birthDate"];
    if (teacherFields.includes(name)) {
      setEditTeacher({
        ...editTeacher,
        teacher: { ...editTeacher.teacher!, [name]: value },
      });
    } else {
      setEditTeacher({ ...editTeacher, [name]: value });
    }
  };

  const handleSaveEdit = async () => {
    if (!editTeacher) return;
    try {
      const userPatch = [
        { op: "replace", path: "/firstName", value: editTeacher.firstName },
        { op: "replace", path: "/lastName", value: editTeacher.lastName },
        { op: "replace", path: "/email", value: editTeacher.email },
        { op: "replace", path: "/phone", value: editTeacher.phone },
      ];
      const teacherPatch = [{ op: "replace", path: "/bio", value: editTeacher.teacher!.bio }];
      await updateUser(editTeacher.userId, userPatch);
      await updateTeacher(editTeacher.userId, teacherPatch);
      showMessage("Teacher updated successfully", "success");
      const res = await getAllTeachers();
      setTeachers(res.data);
      setEditingTeacherId(null);
      setEditTeacher(null);
    } catch (err) {
      showMessage(parseApiError(err), "error");
    }
  };

  const handleDeleteTeacher = async (userId: number) => {
    try {
      const lessons = await getLessonsByTeacherId(userId);
      if (lessons.data.length > 0) {
        showMessage("Cannot delete – this teacher has scheduled lessons", "error");
        return;
      }
      if (!window.confirm("Are you sure you want to delete this teacher?")) return;
      await deleteUser(userId);
      await deleteTeacher(userId);
      setTeachers(prev => prev.filter(t => t.userId !== userId));
      showMessage("Teacher deleted successfully", "success");
    } catch (err) {
      showMessage(parseApiError(err), "error");
    }
  };

  return (
    <div>
      {toast && <Toast type={toast.type} message={toast.message} />}
      <div className={styles.container}>
        <h2 className={styles.title}>Manage Teachers</h2>

        <button className={styles.addBtn} onClick={() => setNewTeacherVisible(prev => !prev)}>
          <FaUserPlus /> {newTeacherVisible ? "Cancel" : " Add New Teacher"}
        </button>

        {newTeacherVisible && (
          <div className={styles.modalOverlay}>
            <div className={styles.modalBox}>
              <h3>Add New Teacher:</h3>
              <form onSubmit={handleAdd} className={styles.form}>
                <input
                  name="userId"
                  type="number"
                  placeholder="Teacher ID"
                  value={newTeacher.userId || ""}
                  onChange={handleChange}
                  className={styles.singleInput}
                  required
                />

                <div className={styles.twoColumnForm}>
                  <input name="firstName" placeholder="First Name" value={newTeacher.firstName} onChange={handleChange} required />
                  <input name="lastName" placeholder="Last Name" value={newTeacher.lastName} onChange={handleChange} required />
                  <input name="phone" placeholder="Phone" value={newTeacher.phone} onChange={handleChange} required />
                  <input name="email" placeholder="Email" value={newTeacher.email} onChange={handleChange} required />
                  <input name="password" type="password" placeholder="Password" value={newTeacher.password} onChange={handleChange} required />
                  <select name="gender" value={newTeacher.teacher!.gender} onChange={handleChange} required>
                    <option value="" disabled>
                      Select Gender
                    </option>
                    <option value="M">Male</option>
                    <option value="F">Female</option>
                  </select>
                  <input
                    name="birthDate"
                    type={birthDateInputType}
                    placeholder="Birth Date"
                    value={newTeacher.teacher!.birthDate}
                    onFocus={() => setBirthDateInputType("date")}
                    onBlur={() => {
                      if (!newTeacher.teacher!.birthDate) setBirthDateInputType("text");
                    }}
                    onChange={handleChange}
                  />
                  <textarea name="bio" placeholder="Biography" value={newTeacher.teacher!.bio} onChange={handleChange} />
                </div>

                <div className={styles.actions}>
                  <button type="submit">Save</button>
                  <button type="button" onClick={() => setNewTeacherVisible(false)}>
                    Cancel
                  </button>
                </div>
              </form>
            </div>
          </div>
        )}

        <div className={styles.cardGrid}>
          {teachers.map(t => {
            const isExpanded = expandedTeacherId === t.userId;

            if (editingTeacherId === t.userId && editTeacher) {
              return (
                <div key={t.userId} className={`${styles.card} ${isExpanded ? styles.expanded : ""}`}>
                  <input name="firstName" value={editTeacher.firstName} onChange={handleEditChange} />
                  <input name="lastName" value={editTeacher.lastName} onChange={handleEditChange} />
                  <input name="email" value={editTeacher.email} onChange={handleEditChange} />
                  <input name="phone" value={editTeacher.phone} onChange={handleEditChange} />
                  <textarea name="bio" value={editTeacher.teacher!.bio} onChange={handleEditChange} />
                  

                  <div className={styles.actions}>
                    <button onClick={handleSaveEdit}>
                      <FaSave />
                    </button>
                    <button onClick={() => setEditingTeacherId(null)}>
                      <FaTimes />
                    </button>
                    <button onClick={() => toggleDetails(t.userId)}>{isExpanded ? "Collapse" : "Details"}</button>
                  </div>
                </div>
              );
            }

            const bio = t.teacher?.bio || "";
            const linesToShow = expandedBioLines[t.userId] || LINES_DEFAULT;
            const bioToShow = bio.substring(0, linesToShow * CHARS_PER_LINE);
            const isBioExpanded = linesToShow > LINES_DEFAULT;
            const isBioLong = bio.length > CHARS_PER_LINE;

            return (
              <div key={t.userId} className={`${styles.card} ${isExpanded ? styles.expanded : ""}`}>
                <div className="content">
                  <div className={styles.icon}>
                    <FiUser />
                  </div>
                  <h4>
                    {t.firstName} {t.lastName}
                  </h4>
                </div>
                <div className={styles.actions}>
                  <button onClick={() => handleEditClick(t)} title="Edit">
                    <FaEdit />
                  </button>
                  <button onClick={() => handleDeleteTeacher(t.userId)} title="Delete">
                    <FaTrash />
                  </button>
                  <button onClick={() => toggleDetails(t.userId)}>{isExpanded ? "Collapse" : "Details"}</button>
                </div>
                {isExpanded && (
                  <>
                    <p>
                      <b>Teacher ID:</b> {t.userId}
                    </p>
                    <p>
                      <b>Email:</b> {t.email}
                    </p>
                    <p>
                      <b>Gender:</b> {t.teacher?.gender === "M" ? "Male" : "Female"}
                    </p>
                    <p>
                      <b>Birth Date:</b> {formatDate(t.teacher?.birthDate)}
                    </p>
                    <p>
                      <b>Bio:</b>{" "}
                      <span style={{ whiteSpace: "pre-line" }}>
                        {isBioExpanded
                          ? bio.match(new RegExp(`.{1,${CHARS_PER_LINE}}`, "g"))?.join("\n")
                          : bioToShow}
                      </span>
                    </p>
                    {isBioLong && (
                      <button
                        onClick={() => toggleBioLines(t.userId)}
                        className={styles.readMoreBtn}
                        type="button"
                      >
                        {isBioExpanded ? " Read Less" : " Read More"}
                      </button>
                    )}
                  </>
                )}
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};

export default ManageTeachers;
