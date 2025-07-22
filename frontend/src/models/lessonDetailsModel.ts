import type { Gender } from "./userModel";

export interface LessonDetails {
    gender?: Gender;
    age?:number;
    specificDate: string;
    dateFrom: string;
    dateTo: string;
    startTime: string;
    endTime:string ;
    status:string;
    subjectId?:number;
    teacherId?:number
    
    



}
 