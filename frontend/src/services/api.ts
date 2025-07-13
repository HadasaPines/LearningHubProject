
import axios from 'axios';
import type { User,LoginFormData, StudentDetails, TeacherDetails } from '../models/userModel';
import type { LessonDetails } from '../models/lessonDetailsModel';
import type { Registration } from '../models/registerationModel';

import type { Lesson } from '../models/lessonModel';
import type { TeacherAvailability } from '../models/availabilityModel';
import type {Payment} from "../models/paymentModel";


import type { NewSubject } from '../models/subjectModel';
import type { Subscription } from '../models/subscriptionModel';
import type { StudentSubscription } from '../models/studentSubscriptionModel';


const api = axios.create({
  baseURL: 'https://localhost:7161/api',
  headers: {
    'Content-Type': "application/json-patch+json",
  },
});

export const addUser =async (userData:User) => {
  return api.post('/User/addUser', userData);
};

export const updateUser = (userId: number, patch: any) =>
  api.patch(`/User/updateUser/${userId}`, patch);
export const deleteUser = (userId: number) => api.delete(`/User/deleteUser/${userId}`);
export const getAllUsers = () => api.get("/User/getAllUsers");


export const addStudent = (studentData:StudentDetails) => {
  return api.post('/Student/addStudent', studentData);
};
export const updateStudent = (studentId: number, patch: any) =>
  api.patch(`/Student/updateStudent/${studentId}`, patch);
export const deleteStudent = (studentId: number) => api.delete(`/Student/deleteStudent/${studentId}`);
export const getAllStudents = () => api.get("/Student/getAllStudents");

export const loginUser = async (loginData: LoginFormData) => {
  return api.get('/User/getUserByIdAndPassword',
    {
       params: {
      userId: loginData.userId,
      password: loginData.password,
    }
    });

 };
export const addLesson =async (lessonData: Omit<Lesson, 'lessonId'>) => {
  return api.post('/Lesson/addLesson', lessonData);}

  export const getAllLessons =async () => {
  return api.get('/Lesson/getAllLessons');
};

export const updateLesson = (lessonId: number, patch: any) =>{
  return api.patch(`/Lesson/updateLesson/${lessonId}`, patch);
};

export const deleteLesson = (lessonId: number) => api.delete(`/Lesson/deleteLesson/${lessonId}`);

export const getLessonsByDetails =async (detailsData:LessonDetails) => {
  return api.get('/Lesson/details', {
 params: {
  Gender: detailsData.gender,
  age: detailsData.age,
  SpecificDate: detailsData.specificDate,
  DateFrom: detailsData.dateFrom,
  DateTo: detailsData.dateTo,
  StartTime: detailsData.startTime,
  EndTime: detailsData.endTime,
  Status: detailsData.status,
  SubjectId: detailsData.subjectId,
  TeacherId: detailsData.teacherId
} });
};



export const getAllTeachers =async () => {
  return api.get('/User/getAllTeachers');
};
export const getAllSubjects =async () => {
  return api.get('/Subject/getAllSubjects');
};
export const addRegistration =async (registration:Registration) => {
  return api.post('/Registration/addRegistration',registration);
};
export const addSubject = (subject: NewSubject) => {
  return api.post(`/Subject/addSubject`, subject);
};
export const deleteSubject = (name: string) => {
  return api.delete(`/Subject/deleteSubjectByName/${name}`);
};
export const updateSubject = (id: number, patch: any) => {
  return api.patch(`/Subject/updateSubject/${id}`, patch);}

export const getStudentToLeeson =async (lessonId:number) => {
  return api.get(`/Lesson/getStudentToLeeson/${lessonId}`);
};

export const deleteRegistrationByLessonId = (lessonId: number) => {
  return api.delete(`/Registration/deleteRegistrationByLessonId/${lessonId}`);

};

export const addTeacher = (teacherData: Omit<TeacherDetails, 'teacherId'> & { teacherId: number}) => {
  return api.post('/Teacher/addTeacher', teacherData);
};

export const updateTeacher = (teacherId: number, patch: any) => {
  return api.patch(`/Teacher/updateTeacher/${teacherId}`, patch);
};
export const deleteTeacher = (teacherId: number) => {
  return api.delete(`/Teacher/deleteTeacher/${teacherId}`);
};
export const getLessonsByTeacherId = async (teacherId: number) => {
  return api.get<Lesson[]>(`/Lesson/getLessonsByTeacherId/${teacherId}`)
;}
export const getLessonsByStudentId = async (studentId: number) => {
  return api.get<Lesson[]>(`/Lesson/getLessonsByStudentId/${studentId}`)
;}


export const getAllAvailabilities = () =>
  api.get("/TeacherAvailability/getAllTeacherAvailability");

export const addAvailability = (data: Omit<TeacherAvailability, "availabilityId">) =>
  api.post("/TeacherAvailability/AddTeacherAvailability", data);

export const updateAvailability = (id: number, patch:any) =>
  api.patch(`/TeacherAvailability/updateTeacherAvailability/${id}`, patch);

export const deleteAvailability = (id: number) =>
  api.delete(`/TeacherAvailability/deleteTeacherAvailability/${id}`);

export const addSubscription = (subscription: Omit<Subscription, "subSubscriptionId">) =>
  api.post("/Subscription/AddSubscription", subscription);

export const getAllSubscriptions = () =>{
  return api.get<Subscription[]>("/Subscription/getAllSubscriptions");
};
export const updateSubscriptionActive = (id: number) =>{
 return api.put(`/Subscription/updateSubscriptionActive/${id}`)
};

export const addStudentSubscription = (
  subscription: Omit<StudentSubscription, "studentSubscriptionId">
) => {
  return api.post("/StudentSubscription/addStudentSubscription", subscription);
};

export const getStudentSubscriptionById = (studentId: number) => {
  return api.get<StudentSubscription[]>(`/StudentSubscription/getStudentSubscriptionsByStudentId/${studentId}`);
};


export const addPayment = (paymentDate:Omit<Payment,"paymentId">) => {
  return api.post(`/Payment/addPayment`,paymentDate);
}

export const updateLessonsUsedForActiveStudentSubscription=(userId:number) => {
  return api.put(`/StudentSubscription/updateLessonsUsedForActiveStudentSubscription/${userId}`);
}
export const getPaymentsByUserId = async (userId: number): Promise<Payment[]> => {
  const response = await api.get(`/Payment/getPaymentsByStudentId/${userId}`);
  return response.data;
};

export default api;

