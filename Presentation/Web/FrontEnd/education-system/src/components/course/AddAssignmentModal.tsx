import React, { useState } from 'react';
import { Modal, Paper, Typography, TextField, Button } from '@mui/material';
import { AddAssignmentRequestModel } from '../../services/assignmentService';

interface AddAssignmentModalProps {
  open: boolean;
  handleClose: () => void;
  handleAddAssignment: (assignment: AddAssignmentRequestModel) => void;
  courseId: number;
}

const AddAssignmentModal: React.FC<AddAssignmentModalProps> = ({ open, handleClose, handleAddAssignment, courseId }) => {
  const [newAssignment, setNewAssignment] = useState({
    title: '',
    description: '',
    maxGrade: '',
    deadline: ''
  });

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setNewAssignment((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    handleAddAssignment({
      courseId,
      title: newAssignment.title,
      description: newAssignment.description,
      maxGrade: newAssignment.maxGrade ? Number(newAssignment.maxGrade) : undefined,
      deadline: new Date(newAssignment.deadline).toISOString(),
      userId: 1 // Replace with actual userId
    });
    handleClose();
  };

  return (
    <Modal open={open} onClose={handleClose}>
      <Paper sx={{ padding: '16px', margin: 'auto', maxWidth: '500px', mt: '10%' }}>
        <Typography variant="h6" gutterBottom>
          Add Assignment
        </Typography>
        <form onSubmit={handleSubmit}>
          <TextField
            fullWidth
            label="Title"
            name="title"
            value={newAssignment.title}
            onChange={handleInputChange}
            required
            sx={{ mb: 2 }}
          />
          <TextField
            fullWidth
            label="Description"
            name="description"
            value={newAssignment.description}
            onChange={handleInputChange}
            multiline
            rows={4}
            sx={{ mb: 2 }}
          />
          <TextField
            fullWidth
            label="Max Grade"
            name="maxGrade"
            value={newAssignment.maxGrade}
            onChange={handleInputChange}
            type="number"
            sx={{ mb: 2 }}
          />
          <TextField
            fullWidth
            label="Deadline"
            name="deadline"
            value={newAssignment.deadline}
            onChange={handleInputChange}
            type="datetime-local"
            sx={{ mb: 2 }}
          />
          <Button type="submit" variant="contained" color="primary">
            Add Assignment
          </Button>
        </form>
      </Paper>
    </Modal>
  );
};

export default AddAssignmentModal;
