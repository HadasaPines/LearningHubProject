// src/api.ts

import axios from 'axios';
import type { RegisterFormData,LoginFormData } from '../models/userModel';

const api = axios.create({
  baseURL: 'https://localhost:7161/api',
  headers: {
    'Content-Type': 'application/json',
  },
});


export const addUser =async (userData: RegisterFormData) => {
  return api.post('/User/addUser', userData);
};

// export const addTeacher = (teacherData: {
//   teacherId: number;
//   gender: "M" | "F";
//   bio?: string;
//   birthDate: string;
// }) => {
//   return api.post('/Teacher/addTeacher', teacherData);
// };

// שליחת פרטי תלמיד
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
export default api;
