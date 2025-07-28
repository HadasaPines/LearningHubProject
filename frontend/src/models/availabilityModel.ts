export interface TeacherAvailability {
  availabilityId: number;
  teacherId: number;
  subjectId: number;
  weekDay: number;
  minAge?: number | null;
  maxAge?: number | null;
  startTime: string;
  endTime: string;  
}
