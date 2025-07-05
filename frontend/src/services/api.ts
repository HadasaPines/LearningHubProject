
import axios from 'axios';
import type { User,LoginFormData } from '../models/userModel';
import type { LessonDetails } from '../models/lessonDetailsModel';
import type { Registration } from '../models/registerationModel';


const api = axios.create({
  baseURL: 'https://localhost:7161/api',
  headers: {
    'Content-Type': 'application/json',
  },
});


export const addUser =async (userData:User) => {
  return api.post('/User/addUser', userData);
};


export const addStudent = (studentData: {
  studentId: number;
  gender: "M" | "F";
  age: number;
  birthDate: string;
}) => {
  return api.post('/Student/addStudent', studentData);
};
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
