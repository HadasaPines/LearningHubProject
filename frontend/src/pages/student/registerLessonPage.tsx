import React, { useState, useEffect } from "react";
import type { Lesson } from "../../models/lessonModel";
import type { LessonDetails } from "../../models/lessonDetailsModel";
import type { Subject } from "../../models/subjectModel";
import type { StudentDetails, User } from "../../models/userModel";
import type { Registration } from "../../models/registerationModel";

import { parseApiError } from "../../utils/apiErrorParser";
import {
  getAllTeachers,
  getAllSubjects,
  getLessonsByDetails,
  addRegistration
} from "../../services/api";

import CalendarView from "../../components/calendarView";
import PaymentOverlay from "../../components/paymentOverlay"; 

const RegisterLessonForm: React.FC = () => {
  const [teachers, setTeachers] = useState<User[]>([]);
  const [subjects, setSubjects] = useState<Subject[]>([]);
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [errorMessages, setErrorMessages] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const user = localStorage.getItem("user");
  if (!user) return <div>User not logged in</div>;
  const userData: User = JSON.parse(user);
  if (userData.role !== "Student") {
    return <div>Access denied. Only students can register for lessons.</div>;
  }
  if(userData.student === undefined) {
    return <div>User is not defined as a student</div>;}
  const student: StudentDetails = userData.student ;

  const [formData, setFormData] = useState<LessonDetails>({
    teacherId: undefined,
    subjectId: undefined,
    startTime: "",
    endTime: "",
    specificDate: "",
    dateFrom: "",
    dateTo: "",
    age: student.age,
    status: "",
    gender: student.gender,
  });

  const [paymentOpen, setPaymentOpen] = useState(false);
  const [selectedLesson, setSelectedLesson] = useState<Lesson | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const teachersRes = await getAllTeachers();
        const subjectsRes = await getAllSubjects();
        setTeachers(teachersRes.data);
        setSubjects(subjectsRes.data);
      } catch (error) {
        setErrorMessages(parseApiError(error));
      }
    };
    fetchData();
  }, []);

  useEffect(() => {
    const fetchLessons = async () => {
      try {
        const response = await getLessonsByDetails(formData);
        setLessons(response.data);
        setErrorMessages(null);
      } catch (error) {
        setErrorMessages(parseApiError(error));
      }
    };
    fetchLessons();
  }, [formData]);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: ["teacherId", "subjectId", "age"].includes(name)
        ? value === "" ? undefined : Number(value)
        : value,
    }));
  };

  const handleOpenPayment = (lesson: Lesson) => {
    setSelectedLesson(lesson);
    setPaymentOpen(true);
  };

  const handlePaymentConfirm = async () => {
    if (!selectedLesson) return;

    const registration: Registration = {
      studentId: userData.userId,
      lessonId: selectedLesson.lessonId,
    };

    try {
      await addRegistration(registration);
      setSuccessMessage("Successfully registered for the lesson!");
      setErrorMessages(null);
    } catch (error) {
      setErrorMessages("Registration error: " + parseApiError(error));
    } 
  };

  return (
    <>
      {errorMessages && (
        <div role="alert" style={{ color: "red" }}>{errorMessages}</div>
      )}
      {successMessage && (
        <div style={{ color: "green" }}>{successMessage}</div>
      )}

      <div dir="rtl" style={{ display: "flex", gap: "2rem", alignItems: "flex-start" }}>
        <form style={{ width: "320px" }}>
          <h2>Filter Lessons</h2>

          <div>
            <label>Teacher:</label>
            <select name="teacherId" value={formData.teacherId ?? ""} onChange={handleChange}>
              <option value="">Select a teacher</option>
              {teachers.map((teacher) => (
                <option key={teacher.userId} value={teacher.userId}>
                  {teacher.firstName} {teacher.lastName}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label>Subject:</label>
            <select name="subjectId" value={formData.subjectId ?? ""} onChange={handleChange}>
              <option value="">Select a subject</option>
              {subjects.map((subject) => (
                <option key={subject.subjectId} value={subject.subjectId}>
                  {subject.name}
                </option>
              ))}
            </select>
          </div>

          <div><label>Start Time:</label><input type="time" name="startTime" value={formData.startTime} onChange={handleChange} /></div>
          <div><label>End Time:</label><input type="time" name="endTime" value={formData.endTime} onChange={handleChange} /></div>
          <div><label>Specific Date:</label><input type="date" name="specificDate" value={formData.specificDate} onChange={handleChange} /></div>
          <div><label>Date From:</label><input type="date" name="dateFrom" value={formData.dateFrom} onChange={handleChange} /></div>
          <div><label>Date To:</label><input type="date" name="dateTo" value={formData.dateTo} onChange={handleChange} /></div>
        </form>

        <CalendarView
          lessons={lessons}
          teachers={teachers}
          subjects={subjects}
          onRegister={handleOpenPayment}
        />
      </div>

      {selectedLesson && (
        <PaymentOverlay
          open={paymentOpen}
          onClose={() => {
            setPaymentOpen(false);
            setSelectedLesson(null);
          }}
          amount={150} 
          onPaymentSuccess={handlePaymentConfirm}
        />
      )}
    </>
  );
};

export default RegisterLessonForm;
