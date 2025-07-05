// export type Role = "Teacher" | "Student" | "Admin";
// export type Gender = "Male" | "Female";

export interface StudentDetails {

  gender: "M" | "F";
  age: number;
  birthDate: string;
}

export interface TeacherDetails {

  gender: "M" | "F";
  bio: string;
  birthDate: string;
}

export interface User {
  userId: number;
  firstName: string;
  lastName: string;
  password: string;
  phone: string;
  email: string;
  role: "Student";
  student: StudentDetails;
  teacher: TeacherDetails;
}

export interface LoginFormData {
  userId: number;
  password: string;
  // email: string;
}
