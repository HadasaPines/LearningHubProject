export interface Lesson {
  lessonId: number;
  teacherId: number;
  subjectId: number;
  lessonDate: string;  
  startTime: string;
  endTime: string;
  minAge: number;
  maxAge: number;
  gender: "M"| "F" ;
  status: string;
}