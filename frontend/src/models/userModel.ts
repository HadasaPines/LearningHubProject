export type Role = "Teacher" | "Student" | "Admin";
export type Gender = "M" | "F";

export interface StudentDetails {
  studentId?: number; 
  gender?: "M" | "F" ;
  age? :number;
  birthDate?: string;
}

export interface TeacherDetails {
teacherId?: number;
  gender?: "M" | "F";
  bio?: string;
  birthDate?: string;
}

export interface User {
  userId: number;
  firstName: string;
  lastName: string;
  password: string;
  phone: string;
  email: string;
  role: "Student"|"Teacher"|"Admin";
  student?: StudentDetails;
  teacher?: TeacherDetails;

}


export interface LoginFormData {
  userId: number;
  password: string;
  // email: string;
}
