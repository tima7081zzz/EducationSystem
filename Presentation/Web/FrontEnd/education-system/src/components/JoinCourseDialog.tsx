import React, { useState } from 'react';
import { Dialog, DialogTitle, DialogContent, TextField, DialogActions, Button } from '@mui/material';

interface JoinCourseDialogProps {
  open: boolean;
  onClose: () => void;
  onJoin: (courseId: string) => void;
}

const JoinCourseDialog: React.FC<JoinCourseDialogProps> = ({ open, onClose, onJoin }) => {
  const [courseId, setCourseId] = useState('');

  const handleJoin = () => {
    onJoin(courseId);
    setCourseId('');
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>Join Course</DialogTitle>
      <DialogContent>
        <TextField
          autoFocus
          margin="dense"
          label="Course ID"
          type="text"
          fullWidth
          value={courseId}
          onChange={(e) => setCourseId(e.target.value)}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button onClick={handleJoin}>Join</Button>
      </DialogActions>
    </Dialog>
  );
};

export default JoinCourseDialog;
