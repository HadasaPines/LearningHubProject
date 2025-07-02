// src/api.ts

import axios from 'axios';
import type { RegisterFormData,LoginFormData } from '../models/userModel';
import type { LessonDetails } from '../models/lessonDetailsModel';
import type { Lesson } from '../models/lessonModel';


const api = axios.create({
  baseURL: 'https://localhost:7161/api',
  headers: {
    'Content-Type': 'application/json',
  },
});


export const addUser =async (userData: RegisterFormData) => {
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

export const getAllTechers =async () => {
  return api.get('/User/getAllTeachers');
};
export const getAllSubjects =async () => {
  return api.get('/Subject/getAllSubjects');
};
export const registerToLesson =async (lesson:Lesson) => {
  return api.post('/Lesson/registerToLesson',lesson);
};

export default api;
