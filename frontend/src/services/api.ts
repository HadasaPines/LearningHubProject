
import axios from 'axios';
import type { User,LoginFormData } from '../models/userModel';
import type { LessonDetails } from '../models/lessonDetailsModel';
import type { Registration } from '../models/registerationModel';
import type { Student,User2 } from '../models/studentModel';





const api = axios.create({
  baseURL: 'https://localhost:7161/api',
  headers: {
    'Content-Type': 'application/json',
  },
});


export const addUser =async (userData:User) => {
  return api.post('/User/addUser', userData);
};
export const addUser2 =async (userData:User2) => {
  return api.post('/User/addUser', userData);
};

export const updateUser = (userId: number, patch: any) =>
  api.patch(`/User/updateUser/${userId}`, patch);
export const deleteUser = (userId: number) => api.delete(`/User/deleteUser/${userId}`);
export const getAllUsers = () => api.get("/User/getAllUsers");


export const addStudent = (studentData: Omit<Student, 'studentId'> & { studentId: number
}) => {
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

export default api;

