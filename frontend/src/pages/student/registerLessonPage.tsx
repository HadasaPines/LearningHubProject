


import React, { useState, useEffect } from "react";
import type { Lesson } from "../../models/lessonModel";
import type { LessonDetails } from "../../models/lessonDetailsModel";
import type { Subject } from "../../models/subjectModel";
import type { StudentDetails, User } from "../../models/userModel";
import styles from "./RegisterLessonForm.module.scss";
import Toast from "../../components/toast";
import {
  updateLessonsUsedForActiveStudentSubscription,
  getAllTeachers,
  getAllSubjects,
  getLessonsByDetails,
  addRegistration,
} from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";
import CalendarView from "../../components/calendarView";
import { useNavigate } from "react-router-dom";
import PaymentOverlay from "../../components/paymentOverlay";

const RegisterLessonForm: React.FC = () => {
  const navigate = useNavigate();

  const [teachers, setTeachers] = useState<User[]>([]);
  const [subjects, setSubjects] = useState<Subject[]>([]);
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [errorMessages, setErrorMessages] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [subscriptionError, setSubscriptionError] = useState(false);

  const user = localStorage.getItem("user");
  if (!user) return <div>User not logged in</div>;
  const userData: User = JSON.parse(user);
  if (userData.role !== "Student") {
    return <div>Access denied. Only students can register for lessons.</div>;
  }
  if (userData.student === undefined) {
    return <div>User is not defined as a student</div>;
  }
  const student: StudentDetails = userData.student;
  const [paymentOpen, setPaymentOpen] = useState(false);
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
    if (errorMessages || successMessage) {
      const timeout = setTimeout(() => {
        setErrorMessages(null);
        setSuccessMessage(null);
        setSubscriptionError(false);
      }, 4000);
      return () => clearTimeout(timeout);
    }
  }, [errorMessages, successMessage]);

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

  const handlePaymentSuccess = async () => {
    setPaymentOpen(false);

    try {
      setSuccessMessage(" successfully!");
    } catch (err) {
      setErrorMessages("Error" + parseApiError(err));
    }
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: ["teacherId", "subjectId", "age"].includes(name)
        ? value === ""
          ? undefined
          : Number(value)
        : value,
    }));
  };

  const handleSubmit = async (lesson: Lesson) => {
    const registration = {
      studentId: userData.userId,
      lessonId: lesson.lessonId,
    };

    try {
      await updateLessonsUsedForActiveStudentSubscription(userData.userId);
      await addRegistration(registration);
      setSuccessMessage(
        "Successfully registered! Subscription usage updated."
      );
      setErrorMessages(null);
      setSubscriptionError(false);

      const response = await getLessonsByDetails(formData);
      setLessons(response.data);
    } catch (error: any) {
      const err = parseApiError(error);

      if (error?.response?.status === 404) {
        setErrorMessages(
          "No active subscription found"
        );
        setSubscriptionError(true);
      } else {
        setErrorMessages("Error registering for lesson: " + err);
        setSubscriptionError(false);
      }

      setSuccessMessage(null);
    }
  };

  return (
    <>
      {errorMessages && (
        <Toast type="error" message={errorMessages}>
          {subscriptionError && (
            <>
              <button className={styles.navigateSubscripation}
                onClick={() => navigate("/student/buySubscriptions")}
              >
                Buy Subscription
              </button>
              <button className={styles.navigatePayment}
                onClick={() => setPaymentOpen(true)}
              >
                Payment for single lesson
              </button>
            </>
          )}

        </Toast>
      )}
      {paymentOpen && (
        <PaymentOverlay
          userId={student.studentId ? student.studentId : 0}
          open={paymentOpen}
          onClose={() => setPaymentOpen(false)}
          amount={90}
          onPaymentSuccess={handlePaymentSuccess}
        />
      )}

      {successMessage && <Toast type="success" message={successMessage} />}

      <div className={styles.container}>
        <aside className={styles.sidebar}>
          <h2>Filter Lessons</h2>
          <form className={styles.formContainer}>
            <div>
              <label>Teacher:</label>
              <select
                name="teacherId"
                value={formData.teacherId ?? ""}
                onChange={handleChange}
              >
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
              <select
                name="subjectId"
                value={formData.subjectId ?? ""}
                onChange={handleChange}
              >
                <option value="">Select a subject</option>
                {subjects.map((subject) => (
                  <option key={subject.subjectId} value={subject.subjectId}>
                    {subject.name}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label>Start Time:</label>
              <input
                type="time"
                name="startTime"
                value={formData.startTime}
                onChange={handleChange}
              />
            </div>
            <div>
              <label>End Time:</label>
              <input
                type="time"
                name="endTime"
                value={formData.endTime}
                onChange={handleChange}
              />
            </div>
            <div>
              <label>Specific Date:</label>
              <input
                type="date"
                name="specificDate"
                value={formData.specificDate}
                onChange={handleChange}
              />
            </div>
            <div>
              <label>Date From:</label>
              <input
                type="date"
                name="dateFrom"
                value={formData.dateFrom}
                onChange={handleChange}
              />
            </div>
            <div>
              <label>Date To:</label>
              <input
                type="date"
                name="dateTo"
                value={formData.dateTo}
                onChange={handleChange}
              />
            </div>
          </form>
        </aside>

        <main className={styles.calendarSection}>
          <CalendarView
            lessons={lessons}
            teachers={teachers}
            subjects={subjects}
            onRegister={handleSubmit}
          />
        </main>
      </div>
    </>
  );
};

export default RegisterLessonForm;
