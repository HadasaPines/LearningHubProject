// src/api.ts

import axios from 'axios';
import type { SignupFormData } from '../models/userModel';

const api = axios.create({
  baseURL: 'https://localhost:7161/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// שליחת משתמש בסיסי
export const addUser =async (userData: SignupFormData) => {
  return api.post('/User/addUser', userData);
};

// שליחת פרטי מורה
export const addTeacher = (teacherData: {
  teacherId: number;
  gender: "M" | "F";
  bio?: string;
  birthDate: string;
}) => {
  return api.post('/Teacher/addTeacher', teacherData);
};

// שליחת פרטי תלמיד
export const addStudent = (studentData: {
  studentId: number;
  gender: "M" | "F";
  age: number;
  birthDate: string;
}) => {
  return api.post('/Student/addStudent', studentData);
};

export default api;
