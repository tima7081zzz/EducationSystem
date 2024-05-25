import React from 'react';
import { Paper, Typography, Button } from '@mui/material';

interface CoursePublicIdProps {
  publicId: string;
  handleCopyPublicId: () => void;
}

const CoursePublicId: React.FC<CoursePublicIdProps> = ({ publicId, handleCopyPublicId }) => {
  return (
    <Paper elevation={3} sx={{ padding: '16px', mb: 3 }}>
      <Typography variant="subtitle1" gutterBottom>
        Course Public ID
      </Typography>
      <Typography variant="body2" sx={{ mb: 2 }}>
        {publicId}
      </Typography>
      <Button variant="outlined" onClick={handleCopyPublicId}>
        Copy it
      </Button>
    </Paper>
  );
};

export default CoursePublicId;
