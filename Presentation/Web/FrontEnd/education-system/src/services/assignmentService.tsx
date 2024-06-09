import axios from '../axiosConfig';

export interface AddAssignmentRequestModel {
  courseId: number;
  maxGrade?: number;
  title: string;
  description?: string;
  userId: number;
  deadline?: string;
}

export interface AttachmentModel {
    id: number;
    name: string;
}
  
  export interface AssignmentModel {
    id: number;
    title: string;
    description?: string;
    createdAt: string;
    deadline: string;
    maxGrade?: number;
    grade?: number;
    attachments: AttachmentModel[];
    status: StudentCourseTaskStatus;
}

export enum StudentCourseTaskStatus {
    NotSubmitted = 0,
    SubmittedLate = 1,
    SubmittedInTime = 2,
  }

  export interface StudentAssignmentOverviewModel {
    userId: number;
    userFullname: string;
    grade: number | null;
  }
  
  export interface GetAssignmentOverviewModel {
    id: number;
    title: string;
    submittedCount: number;
    notSubmittedCount: number;
    gradedCount: number;
    maxGrade: number | null;
    studentAssignmentInfos: StudentAssignmentOverviewModel[];
  }

  export interface GradeAssignmentRequestModel {
    grade: number;
    gradingComment: string | null;
  }

  export interface AssignmentTeacherPreviewModel {
    id: number;
    submissionComment: number | null;
    studentAttachments: AttachmentModel[];
  }
  
  export const getAssignmentOverview = async (assignmentId: number): Promise<GetAssignmentOverviewModel> => {
    const response = await axios.get<GetAssignmentOverviewModel>(`/api/assignment/${assignmentId}/overview`);
    return response.data;
  };

const getAssignmentDetails = async (assignmentId: number): Promise<AssignmentModel> => {
  const response = await axios.get<AssignmentModel>(`/api/assignment/${assignmentId}`);
  return response.data;
};

const addAssignment = async (assignment: AddAssignmentRequestModel): Promise<void> => {
  await axios.post('/api/assignment', assignment);
};

const uploadAttachment = async (assignmentId: number, file: File): Promise<void> => {
    const formData = new FormData();
    formData.append('file', file);
  
    await axios.post(`/api/assignment/${assignmentId}/upload-attachment`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
 };

const submitAssignment = async (assignmentId: number, data: { comment: string }): Promise<void> => {
   await axios.post(`/api/assignment/${assignmentId}/submit`, data);
};

const unsubmitAssignment = async (assignmentId: number): Promise<void> => {
   await axios.post(`/api/assignment/${assignmentId}/unsubmit`);
};

const deleteAttachment = async (attachmentId: number): Promise<void> => {
    await axios.delete(`/api/assignment/attachment/${attachmentId}`);
 };

 export const gradeStudentAssignment = async (assignmentId: number, studentUserId: number, request: GradeAssignmentRequestModel): Promise<void> => {
  await axios.post(`/api/assignment/${assignmentId}/student-user/${studentUserId}/grade`, request);
};

export const getAssignmentTeacherPreview = async (assignmentId: number, studentUserId: number): Promise<AssignmentTeacherPreviewModel> => {
  const response = await axios.get<AssignmentTeacherPreviewModel>(`/api/assignment/${assignmentId}/student-user/${studentUserId}/teacher-preview`);
  return response.data;
};

export { getAssignmentDetails, addAssignment, uploadAttachment, submitAssignment, unsubmitAssignment, deleteAttachment };
