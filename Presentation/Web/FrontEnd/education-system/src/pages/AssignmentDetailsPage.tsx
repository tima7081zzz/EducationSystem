import React, { useState } from 'react';
import { Container, Typography, Grid, Paper, Box, Button, TextField } from '@mui/material';
import { useParams } from 'react-router-dom';

const AssignmentPage: React.FC = () => {
  const { assignmentId } = useParams<{ assignmentId: string }>();
  const isTeacher = false; // This should be derived from your actual data

  const assignment = {
    id: assignmentId,
    title: 'Sample Assignment',
    description: 'This is a sample assignment description.',
    creationDate: '2024-01-01',
    deadlineDate: '2024-01-10',
    maxGrade: 100,
  };

  const [attachment, setAttachment] = useState<File | null>(null);
  const [submissionText, setSubmissionText] = useState('');
  const [error, setError] = useState<string | null>(null);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file && file.size > 10 * 1024 * 1024) {
      setError('File size should not exceed 10MB');
      setAttachment(null);
    } else {
      setError(null);
      setAttachment(file || null);
    }
  };

  const handleSubmit = () => {
    if (submissionText.length > 300) {
      setError('Submission text should not exceed 300 characters');
      return;
    }
    setError(null);
    // Handle submission logic here
    console.log('Submitting assignment with text:', submissionText, 'and attachment:', attachment);
  };

  return (
    <Container>
      <Paper elevation={3} sx={{ padding: '16px', mb: 3 }}>
        <Grid container spacing={3}>
          <Grid item xs={12} md={8} sx={{ display: 'flex', flexDirection: 'column', justifyContent: 'space-between' }}>
            <Box>
              <Typography variant="h5" component="h1" gutterBottom>
                {assignment.title}
              </Typography>
              <Typography variant="body1" component="p" gutterBottom>
                {assignment.description}
              </Typography>
            </Box>
            <Box>
              <Typography variant="body2" color="textSecondary" gutterBottom>
                Created on: {assignment.creationDate}
              </Typography>
              <Typography variant="body2" color="textSecondary" gutterBottom>
                Deadline: {assignment.deadlineDate}
              </Typography>
              {assignment.maxGrade && (
                <Typography variant="body2" color="textSecondary" gutterBottom>
                  Max Grade: {assignment.maxGrade}
                </Typography>
              )}
            </Box>
          </Grid>
          {!isTeacher && (
            <Grid item xs={12} md={4}>
              <Box component="form" noValidate autoComplete="off" sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                <Button
                  variant="contained"
                  component="label"
                >
                  Upload Attachment
                  <input
                    type="file"
                    hidden
                    onChange={handleFileChange}
                  />
                </Button>
                {attachment && <Typography variant="body2">{attachment.name}</Typography>}
                <TextField
                  label="Submission Text"
                  multiline
                  rows={4}
                  value={submissionText}
                  onChange={(e) => setSubmissionText(e.target.value)}
                  inputProps={{ maxLength: 300 }}
                />
                {error && <Typography color="error">{error}</Typography>}
                <Button
                  variant="contained"
                  color="primary"
                  onClick={handleSubmit}
                >
                  Submit
                </Button>
              </Box>
            </Grid>
          )}
        </Grid>
      </Paper>
    </Container>
  );
};

export default AssignmentPage;
