import React, { useState } from 'react';
import { Dialog, DialogTitle, DialogContent, TextField, DialogActions, Button } from '@mui/material';

interface CreateCourseDialogProps {
  open: boolean;
  onClose: () => void;
  onCreate: (course: { name: string; description: string; category: string }) => void;
}

const CreateCourseDialog: React.FC<CreateCourseDialogProps> = ({ open, onClose, onCreate }) => {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [category, setCategory] = useState('');
  const [nameError, setNameError] = useState('');

  const handleCreate = () => {
    if (name.trim() === '') {
        setNameError('Course name is required');
        return;
      }
    onCreate({ name, description, category });
    setName('');
    setDescription('');
    setCategory('');
    setNameError('');
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>Create Course</DialogTitle>
      <DialogContent>
        <TextField
          autoFocus
          margin="dense"
          label="Name"
          type="text"
          fullWidth
          required
          value={name}
          onChange={(e) => {
            setName(e.target.value);
            if (e.target.value.trim() !== '') {
              setNameError('');
            }
          }}
          error={!!nameError}
          helperText={nameError}
        />
        <TextField
          margin="dense"
          label="Description"
          type="text"
          fullWidth
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />
        <TextField
          margin="dense"
          label="Category"
          type="text"
          fullWidth
          value={category}
          onChange={(e) => setCategory(e.target.value)}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button onClick={handleCreate}>Create</Button>
      </DialogActions>
    </Dialog>
  );
};

export default CreateCourseDialog;
