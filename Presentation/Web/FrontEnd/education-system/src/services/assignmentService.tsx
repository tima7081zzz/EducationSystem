import axios from 'axios';

export interface AddAssignmentRequestModel {
  courseId: number;
  maxGrade?: number;
  title: string;
  description?: string;
  userId: number;
  deadline: string;
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
    attachments: AttachmentModel[];
    status: StudentCourseTaskStatus;
}

export enum StudentCourseTaskStatus {
    NotSubmitted = 0,
    SubmittedLate = 1,
    SubmittedInTime = 2,
  }

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

export { getAssignmentDetails, addAssignment, uploadAttachment, submitAssignment, unsubmitAssignment, deleteAttachment };
