import React, { useEffect, useState } from "react";
import styles from "./studentDashboard.module.scss";
import type { User } from "../../models/userModel";
import type { Lesson } from "../../models/lessonModel";
import type { StudentSubscription } from "../../models/studentSubscriptionModel";
import type { Subscription } from "../../models/subscriptionModel";
import type { Payment } from "../../models/paymentModel";
import type { Subject } from "../../models/subjectModel";

import {
  getLessonsByStudentId,
  getStudentSubscriptionById,
  getSubscriptionById,
  getPaymentsByUserId,
  getAllSubjects,
  getAllTeachers,
} from "../../services/api";

import { CreditCard, CalendarCheck, CheckCircle2, Info} from "lucide-react";
import { FaHandPointUp } from "react-icons/fa";


import Toast from "../toast";

const tips = [
  "Stay consistent with your study schedule for better results.",
  "Review your lesson history regularly to track progress.",
  "Make sure your profile details are always up to date.",
  "Use your subscription wisely to attend more lessons.",
  "Don’t wait for the last minute to register for a lesson.",
  "Check your payment history to stay organized.",
  "Short daily study sessions are more effective than long ones once a week.",
  "Take breaks during long study sessions to stay focused.",
  "Use the dashboard to get an overview of your learning.",
  "Prepare questions in advance before attending a lesson.",
  "If you miss a lesson, reschedule as soon as possible.",
  "Check for new available subscriptions regularly.",
  "Clear goals for each lesson help you stay on track.",
  "Ask your teachers for feedback to improve faster.",
  "Plan your learning week ahead every Sunday night.",
  "Your progress is visible—don’t forget to celebrate small wins.",
 
];

const DashboardComponent: React.FC = () => {
  const [userData, setUserData] = useState<User | null>(null);
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [studentSubscriptions, setStudentSubscriptions] = useState<StudentSubscription[]>([]);
  const [subscriptions, setSubscriptions] = useState<Subscription[]>([]);
  const [payments, setPayments] = useState<Payment[]>([]);
  const [subjects, setSubjects] = useState<Subject[]>([]);
  const [teachers, setTeachers] = useState<User[]>([]);
  const [error, setError] = useState<string | null>(null);

  const [tipIndex, setTipIndex] = useState(0); 

 
  useEffect(() => {
    const interval = setInterval(() => {
      setTipIndex((prev) => (prev + 1) % tips.length);
    }, 15000);
    return () => clearInterval(interval);
  }, []);

  useEffect(() => {
    const user = localStorage.getItem("user");
    if (user) setUserData(JSON.parse(user));
  }, []);

  useEffect(() => {
    if (!userData) return;

    const fetchData = async () => {
      try {
        const [lessonsRes, studentSubsRes, paymentsRes, subjectsRes,
            teachersRes
        ] = await Promise.all([
          await getLessonsByStudentId(userData.userId),
          await  getStudentSubscriptionById(userData.userId),
          await getPaymentsByUserId(userData.userId),
           await getAllSubjects(),
          await getAllTeachers(),
        ]);

        setLessons(lessonsRes.data);
        setStudentSubscriptions(studentSubsRes.data);
        setPayments(paymentsRes);
       setSubjects(subjectsRes.data);
        setTeachers(teachersRes.data);

        const uniqueSubIds = [...new Set(studentSubsRes.data.map((sub) => sub.subscriptionId))];
        const subDetails = await Promise.all(uniqueSubIds.map(getSubscriptionById));
        setSubscriptions(subDetails.map((res) => res.data));
      } catch (err) {
        setError("Error loading dashboard data");
      }
    };

    fetchData();
  }, [userData]);

  const getSubjectName = (id: number) => {
    const subject = subjects.find((s) => s.subjectId === id);
    return subject ? subject.name : "Unknown";
  };

 const getTeacherName = (id: number) => {
  if (!teachers || teachers.length === 0) return "Unknown"; 
  const teacher = teachers.find((t) => t.userId === id);
  return teacher ? `${teacher.firstName} ${teacher.lastName}` : "Unknown";
};

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    const day = date.getDate().toString().padStart(2, "0");
    const month = (date.getMonth() + 1).toString().padStart(2, "0");
    const year = date.getFullYear();
    return `${day}-${month}-${year}`;
  };

  if (!userData) return <div className={styles.dashboard}>Loading user info...</div>;

  const futureLessons = lessons.filter((lesson) => new Date(lesson.lessonDate) > new Date());
  const completedLessons = lessons.filter((lesson) => new Date(lesson.lessonDate) <= new Date());
  const totalPayments = payments.reduce((sum, p) => sum + p.amount, 0);
  const activeSub = studentSubscriptions.find((s) => s.isActive);
  const activeSubDetails = subscriptions.find((s) => s.subscriptionId === activeSub?.subscriptionId);

  return (
    <div>
      {error && <Toast type="error" message={error} />}

      <div className={styles.dashboard}>
        <h2 className={styles.title}>Welcome Back, {userData.firstName}!</h2>
        <p className={styles.subtitle}>Here’s what’s coming up for you:</p>

        <div className={styles.mainGrid}>
          <div className={styles.leftColumn}>
            {futureLessons.length > 0 ? (
              <div className={styles.lessonsList}>
                {futureLessons.map((lesson, index) => (
                  <div className={styles.lessonCard} key={index}>
                    <div className={styles.pin}></div>
                    <div className={styles.cardPaper}>
                      <div className={styles.lessonDate}>{formatDate(lesson.lessonDate)}</div>
                      <div className={styles.lessonSubject}>{getSubjectName(lesson.subjectId)}</div>
                      <div className={styles.lessonTecher}>{getTeacherName(lesson.teacherId)}</div>
                      <div className={styles.lessonTime}>
                        {lesson.startTime} - {lesson.endTime}
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            ) : (
             <div className={styles.noLessonsCard}>
  <div className={styles.pin}></div>
  <div className={styles.cardPaper}>
    <p className={styles.noLessonsText}>No lessons for you</p>

    <div className={styles.inviteContainer}>
      <FaHandPointUp className={styles.handIcon} />
      <span className={styles.inviteText}>Looks empty here... Let's book lesson!</span>
      <FaHandPointUp className={styles.handIcon} />
    </div>
  </div>
</div>
            )}
          </div>

          <div className={styles.rightColumn}>
            <div className={styles.card}>
              <div className={styles.verticalHeader}>
                <CalendarCheck size={16} />
                <span>Subscription</span>
              </div>
              <div className={styles.cardContent}>
                {activeSub ? (
                  <>
                    <p>
                      <strong>Name:</strong> {activeSubDetails?.name}
                    </p>
                    <p>
                      <strong>Used:</strong> {activeSub.lessonsUsed} / {activeSubDetails?.lessonCount}
                    </p>
                  </>
                ) : (
                  <p>No active subscription</p>
                )}
              </div>
            </div>

            <div className={styles.card}>
              <div className={styles.verticalHeader}>
                <CreditCard size={16} />
                <span>Payments</span>
              </div>
              <div className={styles.cardContent}>
                <p>
                  <strong>Total sum:</strong> {totalPayments.toFixed(2)} ₪
                </p>
              </div>
            </div>

            <div className={styles.card}>
              <div className={styles.verticalHeader}>
                <CheckCircle2 size={16} />
                <span>Completed</span>
              </div>
              <div className={styles.cardContent}>
                <p>
                  <strong>Lesson completed:</strong> {completedLessons.length}
                </p>
              </div>
            </div>
          </div>
        </div>

        <div className={styles.tip}>
          <Info size={20} />
          <strong>Tip for you:</strong> {tips[tipIndex]}
        </div>
      </div>
    </div>
  );
};

export default DashboardComponent;
