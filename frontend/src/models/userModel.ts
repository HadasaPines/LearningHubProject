export type Role = "Teacher" | "Student" | "Admin";
export type Gender = "Male" | "Female";

export interface SignupFormData {
  userId: number;
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  password: string;
  role: Role;
  teacherDetails?: {
    gender: Gender;
    bio?: string;
    birthDate: string;
  };
  studentDetails?: {
    gender: Gender;
    age: number;
    birthDate: string;
  };
}
